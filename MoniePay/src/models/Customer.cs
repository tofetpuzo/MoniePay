// SPDX-License-Identifier: Apache-2.0
/*
 * Customer profile aggregate, including KYC and tenant scoping.
 *
 * Copyright (c) 2026, MoniePay
 */

using MoniePay.src.auth;

namespace MoniePay.src.models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsVerifed { get; set; }
        public bool IsBlackListed { get; set; }
        public string KycLevel { get; set; } = "Tier 1";
        // Explicit FK to the owning Identity user. Previously a shadow property,
        // which meant ownership could not be checked without loading the
        // navigation - so nothing stopped one user paying from another's
        // customer account. Mapped to the existing "userId" column.
        public Guid? UserId { get; set; }
        public User? user { get; set; } = default!;
        public ICollection<PaymentMethods> PaymentMethods { get; set; } = new List<PaymentMethods>();
        public Customer() { }

        public Customer(string? email, string? phoneNumber, string? firstName, string? lastName, string? city,
            string? zipCode, string? state, string? country, DateTime createAt, bool? isVerified, string? kycLevel)
        {
            Id = Guid.NewGuid();
            Email = email ?? string.Empty;
            PhoneNumber = phoneNumber ?? string.Empty;
            FirstName = firstName ?? string.Empty;
            LastName = lastName ?? string.Empty;
            City = city ?? string.Empty;
            ZipCode = zipCode ?? string.Empty;
            State = state ?? string.Empty;
            Country = country ?? string.Empty;
            TenantId = Guid.NewGuid();
            CreateAt = createAt;
            UpdateAt = createAt;
            IsVerifed = isVerified ?? false;
            IsBlackListed = false;
            KycLevel = kycLevel ?? "Tier 1";
        }
    }
}
