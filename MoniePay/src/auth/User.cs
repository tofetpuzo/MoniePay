// SPDX-License-Identifier: Apache-2.0
/*
 * Application user backed by ASP.NET Identity.
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace MoniePay.src.auth
{
    public class User : IdentityUser<Guid>
    {

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
