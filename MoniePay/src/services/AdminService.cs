// SPDX-License-Identifier: Apache-2.0
/*
 * AdminService onboarding: admin service for moniepay
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Identity;
using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.shared;

namespace MoniePay.src.services
{

    public interface IAdminService
    {
        Task<User> RegisterAdminAsync(CreateUserRequest request);
        Task<User> GetAdmin(Guid? adminId);
    }

    public class AdminService(UserManager<User> userManager, AppDbContext db, RegisterUserService registerUserService) : IAdminService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _db = db;
        private readonly RegisterUserService _registerUserService = registerUserService;

        // Create the Identity user and the admin profile in a single transaction.
        // If either step fails, nothing is committed.
        public async Task<User> RegisterAdminAsync(CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            var admin = await _registerUserService.RegisterUserAsync(request, Roles.RoleType.Admin);
            if (admin is null) throw new InvalidOperationException("Admin registration failed.");

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
    }
}