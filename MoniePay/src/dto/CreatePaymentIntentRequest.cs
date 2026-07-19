// SPDX-License-Identifier: Apache-2.0
/*
 * DTO for inbound customer-onboarding requests. Flat shape so the client sends
 * one body (no nested Identity user, no separate password query param).
 *
 * Copyright (c) 2026, MoniePay
 */

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static MoniePay.src.models.Customer;
namespace MoniePay.src.dto
{
    public class CreatePaymentIntentRequest
    {
        [JsonPropertyName("username")]
        [Required(ErrorMessage = "username is required")]
        public required string Username { get; set; }

        [JsonPropertyName("Currency")]
        [Required(ErrorMessage = "Currency is required")]
        public required string Currency { get; set; }

        public Customer customer { get; set; } 

    }
}
