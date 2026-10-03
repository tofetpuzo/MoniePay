// SPDX-License-Identifier: Apache-2.0
/*
 * Customer onboarding: creates the Identity user and the customer profile
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

using MoniePay.src.auth;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class CreateUserRequest
    {

        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; } = new Guid();

        [JsonPropertyName("Username")]
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        [Required(ErrorMessage = "password is required")]
        public required string password { get; set; }

        [JsonPropertyName("Email")]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [JsonProperty("roleFlags")]
        public Roles.RoleType RoleFlags { get; set; }

        [JsonProperty("createdOn")]
        public DateTime createdOn { get; set; } = DateTime.UtcNow;

        [JsonProperty("isUserActive")]
        public bool isUserActive { get; set; }
    }
}
