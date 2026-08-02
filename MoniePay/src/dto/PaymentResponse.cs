// SPDX-License-Identifier: Apache-2.0
/*
 * Outbound view of a payment.
 *
 * Deliberately does NOT expose the Payments entity itself. Serialising the
 * entity would walk PaymentAttempts -> Customer -> user (Identity) and reach
 * the password hash, and would cycle on Customer <-> PaymentMethods.
 * Adding a field here is an explicit act; adding one to the entity is not.
 *
 * Copyright (c) 2026, MoniePay
 */

using System.Text.Json.Serialization;
using MoniePay.src.models;

namespace MoniePay.src.dto
{
    public class PaymentResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("paymentIntentId")]
        public Guid PaymentIntentId { get; set; }

        [JsonPropertyName("providerReference")]
        public string ProviderReference { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("isFinal")]
        public bool IsFinal { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>Maps entity -> wire. The only place this translation happens.</summary>
        public static PaymentResponse From(Payments payment) => new()
        {
            Id = payment.Id,
            PaymentIntentId = payment.PaymentIntentId,
            ProviderReference = payment.ProviderReference,
            Status = payment.Status,
            IsFinal = payment.IsFinal,
            CreatedAt = payment.CreatedAt
        };
    }
}
