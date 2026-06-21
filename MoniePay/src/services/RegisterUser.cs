// SPDX-License-Identifier: Apache-2.0
/*
 * DTO for inbound user registration requests.
 *
 * Copyright (c) 2026, MoniePay
 */

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.services
{
    public sealed class RegisterUser
    {
        [JsonPropertyName("username")]
        [Required(ErrorMessage = "username is required")]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        [Required(ErrorMessage = "password is required")]
        public required string Password { get; set; }

        [JsonPropertyName("email")]
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "email must be a valid email address")]
        public required string Email { get; set; }
    }
}
