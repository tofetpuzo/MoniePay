// SPDX-License-Identifier: Apache-2.0
/*
 * Customer onboarding: creates the Identity user and the customer profile
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Identity;
using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;

namespace MoniePay.src.services
{
    public interface ICustomerService
    {
        Task<Customer> RegisterCustomerAsync(RegisterCustomerRequest request);
        Task<Customer> GetCustomer(Guid? customerId);
    }

    public class CustomerService : ICustomerService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _db;

        public CustomerService(UserManager<User> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // Create the Identity user and the customer profile in a single transaction.
        // If either step fails, nothing is committed.
        public async Task<Customer> RegisterCustomerAsync(RegisterCustomerRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await using var transaction = await _db.Database.BeginTransactionAsync();

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                RoleFlags = Roles.RoleType.Customer,
            };

            IdentityResult res = await _userManager.CreateAsync(user, request.Password);
            if (!res.Succeeded)
            {
                var errors = string.Join("; ", res.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Customer registration failed - {errors}");
            }

            // The user now has an Id, so we can link a role row to it.
            _db.Roles.Add(new Roles(Roles.RoleType.Customer) { UserId = user.Id });

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                TenantId = Guid.NewGuid(),
                Email = request.Email,
                PhoneNumber = request.PhoneNumber ?? string.Empty,
                Address = request.Address ?? string.Empty,
                FirstName = request.FirstName ?? string.Empty,
                LastName = request.LastName ?? string.Empty,
                City = request.City ?? string.Empty,
                State = request.State ?? string.Empty,
                Country = request.Country ?? string.Empty,
                ZipCode = request.ZipCode ?? string.Empty,
                KycLevel = request.KycLevel ?? "Tier 1",
                user = user,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            return customer;
        }

        public async Task<Customer> GetCustomer(Guid? customerId)
        {
            ArgumentNullException.ThrowIfNull(customerId);

            try
            {
                var customer = await _db.FindAsync<Customer>(customerId) ?? throw new KeyNotFoundException("cannot find customer");
                return customer;
            }
            catch
            (Exception ex)
            {
                throw new Exception(null, ex);

            }
        }
    }


}
