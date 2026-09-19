// SPDX-License-Identifier: Apache-2.0
/*
 * Customer onboarding: creates the Identity user and the customer profile
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class DepositResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        public static DepositResponse From(CreateDepositRequest depositRequest) => new()
        {
            Id = new Guid(),
            Amount = depositRequest.Amount,
            Status = depositRequest.Status,
            Currency = depositRequest.currency,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
