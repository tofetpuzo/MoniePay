// SPDX-License-Identifier: Apache-2.0
/*
 * User registration, login, and JWT issuance.
 *
 * Copyright (c) 2026, MoniePay
 */

using MD5Hash;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MoniePay.src.auth;
using MoniePay.src.services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static IdentityService;

public interface IIdentityService
{
    // Return the actual token model data, not an HTTP result
    Task<AccessTokenResponse> LoginAsync(User user, AuthHelpers authHelpers);
    Task<IdentityResult> RegisterUserAsync(RegisterUser userDetails);
}

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration; // Loads secret keys from appsettings.json
    }

    public async Task<AccessTokenResponse> LoginAsync(User users, AuthHelpers authHelpers)
    {

        // 1. Validate User Credentials
        if (string.IsNullOrEmpty(users.username) || string.IsNullOrEmpty(users.password))
        {
            if (string.IsNullOrEmpty(users.username))
            {
                throw new ArgumentException("Username (email) must not be null or empty.", nameof(users));
            }
            if (string.IsNullOrEmpty(users.password))
            {
                throw new ArgumentException("Password(password) must not be null or empty", nameof(users));
            }
        }

        var user = await _userManager.FindByNameAsync(users.username);

        // Dynamically append authorization roles assigned to this user
        var authClaims = await AuthHelpers.BuildUserClaimsAsync(user, null);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        string salt = user.userId.ToString();
        string md5Password = user.password.GetMD5WithSalt(salt, EncodingType.UTF8);
        // Verify the hashed password securely [5]
        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, md5Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // Fetch security properties from system settings
        var secretKey = _configuration["JwtSettings:SecretKey"] ?? "PLM0OKN9IJB8UHVGY76TFCXDR54ESZAWQ23BNMKHYRTEWDCBNA";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var expirationMinutes = 60;
        var tokenExpiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        // Build the cryptographic security descriptor object [2]
        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            expires: tokenExpiration,
            claims: authClaims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        // Serialize the structured token data into a raw string [2]
        var serializedAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        // Create an unpredictable, unique refresh sequence string
        var rawRefreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        // (Optional) Save rawRefreshToken to database here linked to the User.Id to validate later

        // Satisfy the required init-only fields of AccessTokenResponse
        return new AccessTokenResponse
        {
            AccessToken = serializedAccessToken,
            ExpiresIn = TimeSpan.FromMinutes(expirationMinutes).Minutes,
            RefreshToken = rawRefreshToken
        };
    }

    public class AuthHelpers
    {
        // Build claims from user data and role flags; optionally include Identity roles from UserManager<User>
        public static async Task<List<Claim>> BuildUserClaimsAsync(
            User user,
            UserManager<User>? userManager = null)
        {
            var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), // IdentityUser.Id
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // Add claims from flags enum (RoleFlags on User)
            if (user.RoleFlags != Roles.RoleType.None)
            {
                foreach (Roles.RoleType r in Enum.GetValues(typeof(Roles.RoleType)))
                {
                    if (r == Roles.RoleType.None) continue;
                    if (user.RoleFlags.HasFlag(r))
                        authClaims.Add(new Claim(ClaimTypes.Role, r.ToString()));
                }
            }

            // Include ASP.NET Identity roles if UserManager is provided
            if (userManager != null)
            {
                var identityRoles = await userManager.GetRolesAsync(user);
                foreach (var role in identityRoles)
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            return authClaims;
        }
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterUser userDetails)
    {
        var user = new User
        {
            UserName = userDetails.Username,
            Email = userDetails.Email,
        };

        IdentityResult res = await _userManager.CreateAsync(user, userDetails.Password);
        if (!res.Succeeded)
        {
            var errors = string.Join("; ", res.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Registration failed - {errors}");
        }
        return res;
    }
}