// SPDX-License-Identifier: Apache-2.0
/*
 * DTO for inbound customer-onboarding requests. Flat shape so the client sends
 * one body (no nested Identity user, no separate password query param).
 *
 * Copyright (c) 2026, MoniePay
 */

using MoniePay.src.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace MoniePay.src.dto
{
    public class CreatePaymentIntentRequest
    {
        [JsonPropertyName("username")]
        [Required(ErrorMessage = "username is required")]
        public required string Username { get; set; }

        [JsonPropertyName("Currency")]
        [Required(ErrorMessage = "Currency is required")]
        public required Currency currency { get; set; }

        [JsonPropertyName("customerId")]
        [Required(ErrorMessage = "customerId is required")]
        public Guid customerId { get; set; }

        [JsonPropertyName("Amount")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public Channel channel { get; set; } = Channel.API;

        [JsonPropertyName("idempotencyKey")]
        [Required(ErrorMessage = "idempotencyKey is required")]
        public required string IdempotencyKey { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethodId")]
        public Guid? PaymentMethodId { get; set; }

        [JsonPropertyName("confirm")]
        public bool Confirm { get; set; } = true;
    }
}
