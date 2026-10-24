// SPDX-License-Identifier: Apache-2.0
/*
 * Cashier DTO: creates the Cashier user atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class CashierDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("amount")]
        public Decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

    }
}
