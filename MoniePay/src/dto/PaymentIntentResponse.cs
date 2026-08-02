// SPDX-License-Identifier: Apache-2.0
/*
 * Outbound view of a payment intent.
 *
 * The PaymentIntents entity carries Customer, Payment, Transaction, Payout and
 * PaymentAttempts navigations. Serialising it would reach Customer -> user
 * (Identity) and cycle on Customer <-> PaymentMethods, so it is never returned
 * directly. This carries the Id the caller needs and nothing else.
 *
 * Copyright (c) 2026, MoniePay
 */

using System.Text.Json.Serialization;
using MoniePay.src.models;

namespace MoniePay.src.dto
{
    public class PaymentIntentResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Populated when the intent was created with confirm = true. Null otherwise.
        /// Nesting a DTO is safe - PaymentResponse has no navigation properties, so
        /// there is nothing for the serialiser to cycle on.
        /// </summary>
        [JsonPropertyName("payment")]
        public PaymentResponse? Payment { get; set; }

        public static PaymentIntentResponse From(PaymentIntents intent) => new()
        {
            Id = intent.Id,
            CustomerId = intent.CustomerId,
            Amount = intent.Amount,
            Currency = intent.Currency,
            Status = intent.Status,
            Reference = intent.Reference,
            CreatedAt = intent.CreatedAt
        };
    }
}
