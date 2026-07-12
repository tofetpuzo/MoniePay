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
        // Identity stores hashed passwords in PasswordHash. Never persist raw passwords.
        // Keep this only as a transient property for incoming registration data; do not
        // expose it on outbound responses ([JsonIgnore] prevents leaking the hash).
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public string? password
        {
            get => PasswordHash;
            set => PasswordHash = value;
        }

        [JsonProperty("isActive")]
        public bool? isActive { get; set; }

        [JsonProperty("isAdmin")]
        public bool? isAdmin { get; set; }

        // Role rows linked to this user via Roles.UserId (one-to-many).
        [JsonProperty("roles")]
        public List<Roles> roles { get; set; } = new();

        [JsonProperty("roleFlags")]
        public Roles.RoleType RoleFlags { get; set; } = Roles.RoleType.None;

        [JsonProperty("createdOn")]
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
    }
}
