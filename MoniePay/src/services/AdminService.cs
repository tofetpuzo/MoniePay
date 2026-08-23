using Microsoft.AspNetCore.Identity;
using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.dto;

namespace MoniePay.src.services
{

    public interface IAdminService
    {
        Task<User> RegisterAdminAsync(CreateAdminRequest request);
        Task<User> GetAdmin(Guid? adminId);
    }

    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _db;

        public AdminService(UserManager<User> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // Create the Identity user and the admin profile in a single transaction.
        // If either step fails, nothing is committed.
        public async Task<User> RegisterAdminAsync(CreateAdminRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var admin = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.Username,
                Email = request.Email,
                isActive = true,
                isAdmin = true,
                RoleFlags = Roles.RoleType.Admin,
            };

            IdentityResult res = await _userManager.CreateAsync(admin, request.password);
            if (!res.Succeeded)
            {
                var errors = string.Join("; ", res.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Admin registration failed - {errors}");
            }

            // The user now has an Id, so we can link a role row to it.
            _db.AppRoles.Add(new Roles(Roles.RoleType.Admin) { UserId = admin.Id });

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            return admin;
        }

        public async Task<User> GetAdmin(Guid? adminId)
        {
            ArgumentNullException.ThrowIfNull(adminId);

            try
            {
                var admin = await _db.FindAsync<User>(adminId.Value) ?? throw new KeyNotFoundException("cannot find admin");
                return admin;
            }
            catch
            (Exception ex)
            {
                throw new Exception(null, ex);

            }
        }

        //TOD0: create an account number for customer and add money 
    }
}
