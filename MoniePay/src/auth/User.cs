// SPDX-License-Identifier: Apache-2.0
/*
 * Application user backed by ASP.NET Identity.
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoniePay.src.auth
{
    public class User : IdentityUser<Guid>
    {
        // Aliases that delegate to the Identity-provided columns.
        // [NotMapped] tells EF Core not to create a separate database column.
        // [JsonIgnore] on the inherited UserName/Email/Id keeps JSON output lowercase only.

        [NotMapped]
        [JsonProperty("userId")]
        public Guid userId
        {
            get => Id;
            set => Id = new Guid();
        }

        [NotMapped]
        [JsonProperty("username")]
        public string? username
        {
            get => UserName;
            set => UserName = value;
        }

        [NotMapped]
        [JsonProperty("email")]
        public string? email
        {
            get => Email;
            set => Email = value;
        }

        // Identity stores hashed passwords in PasswordHash. Never persist raw passwords.
        // Keep this only as a transient property for incoming registration data; do not
        // expose it on outbound responses.
        [NotMapped]
        [JsonProperty("password")]
        public string? password
        {
            get => PasswordHash;
            set => PasswordHash = value;
        }

        [JsonProperty("isActive")]
        public bool? isActive { get; set; }

        [JsonProperty("isAdmin")]
        public bool? isAdmin { get; set; }

        [JsonProperty("roles")]
        public List<Roles> roles { get; set; } = new();

        [JsonProperty("roleFlags")]
        public Roles.RoleType RoleFlags { get; set; } = Roles.RoleType.None;

        [JsonProperty("createdOn")]
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
    }
}
