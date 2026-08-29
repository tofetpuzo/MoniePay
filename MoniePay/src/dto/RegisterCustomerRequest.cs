// SPDX-License-Identifier: Apache-2.0
/*
 * DTO for inbound customer-onboarding requests. Flat shape so the client sends
 * one body (no nested Identity user, no separate password query param).
 *
 * Copyright (c) 2026, MoniePay
 */

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public sealed class RegisterCustomerRequest
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

        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("zipCode")]
        public string? ZipCode { get; set; }

        [JsonPropertyName("kycLevel")]
        public string? KycLevel { get; set; }

        [JsonPropertyName("currency")]
        [Required(ErrorMessage = "currency is required")]
        public required Currency Currency { get; set; }

        [JsonPropertyName("AccountType")]
        [Required(ErrorMessage = "AccountType is required")]
        public required AccountType AccountType { get; set; }
    }
}
