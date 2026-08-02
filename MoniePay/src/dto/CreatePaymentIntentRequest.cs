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

        /// <summary>
        /// Stored payment method funding the debit. Null means use the customer's
        /// default ledger account.
        /// </summary>
        [JsonPropertyName("paymentMethodId")]
        public Guid? PaymentMethodId { get; set; }

        /// <summary>
        /// True  = create the intent and settle it immediately (one round trip).
        /// False = create the intent only; settle later via POST /payments.
        /// Keep the option, because card/bank top-ups can be pending.
        /// </summary>
        [JsonPropertyName("confirm")]
        public bool Confirm { get; set; } = true;

        // status / CreatedAt / LastUpdatedAt removed: server-owned. A caller that
        // can post its own status could declare its own payment successful.
        //
        // Payment / PaymentAttempts / Transaction / Payout stay absent. Everything
        // the payment needs (customer, amount, currency) is already on this request,
        // so nesting the entity was never necessary - and it reached
        // Customer -> user (Identity) and cycled on Customer <-> PaymentMethods.
    }
}
