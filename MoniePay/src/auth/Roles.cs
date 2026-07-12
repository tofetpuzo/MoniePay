// SPDX-License-Identifier: Apache-2.0
/*
 * Role definitions and flag-enum for role-based access control.
 *
 * Copyright (c) 2026, MoniePay
 */
using System.Text.Json.Serializationwh
namespace MoniePay.src.auth
{
    public class Roles
    {
        public Roles() { }
        public Roles(RoleType roleName)
        {
            this.RoleName = roleName;
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public RoleType RoleName { get; set; }

        // Foreign key to the owning user (Users.Id). A role row is created
        // after the user is persisted, so this is always populated.
        public Guid UserId { get; set; }

        // Back-reference for EF only; ignored by the serializer to avoid a
        // User -> roles -> User cycle in responses.
        [JsonIgnore]
        public User? User { get; set; }

        [Flags]
        public enum RoleType
        {
            None = 0,
            Admin = 1 << 0, // 1
            Customer = 1 << 1, // 2
            Merchant = 1 << 2, // 4
            Audit = 1 << 3, // 8
            Finance = 1 << 4, // 16
            Support = 1 << 5, // 32
            CustomerRep = 1 << 6  // 64
        }
    }
}
