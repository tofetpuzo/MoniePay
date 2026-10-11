// SPDX-License-Identifier: Apache-2.0
/*
 * sharedservice onboarding: common user register function
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Identity;
using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.dto;

namespace MoniePay.src.shared
{
    public class UserService(AppDbContext db, UserManager<User> userManager)
    {
        private readonly AppDbContext _appDbContext = db;
        private readonly UserManager<User> manager = userManager;

        // register user service
        public async Task<User> RegisterUserAccountAsync(CreateUserRequest request, Roles.RoleType roleType)
        {
            ArgumentNullException.ThrowIfNull(request);
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Username,
                    Email = request.Email,
                    isActive = true,
                    isAdmin = false,
                    RoleFlags = roleType,
                };


                if (roleType == Roles.RoleType.Admin)
                {
                    user.isAdmin = true;
                }

                IdentityResult res = await manager.CreateAsync(user, request.password);
                if (!res.Succeeded)
                {
                    var errors = string.Join("; ", res.Errors.Select(e => $"{e.Code}: {e.Description}"));
                    throw new InvalidOperationException($"Admin registration failed - {errors}");
                }

                // The user now has an Id, so we can link a role row to it.
                _appDbContext.AppRoles.Add(new Roles(roleType) { UserId = user.Id });

                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUser(Guid? userId)
        {
            if (!userId.HasValue)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                var user = await _appDbContext.FindAsync<User>(userId) ?? throw new KeyNotFoundException("cannot find user");
                return user;
            }
            catch
            (Exception ex)
            {
                throw new Exception(null, ex);
            }
        }
    }
}
