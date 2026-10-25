// SPDX-License-Identifier: Apache-2.0
/*
 * FundingCashierResponse: creates the response 
 * for cashier after casher pays or receives cash 
 * atomically.
 * Copyright (c) 2026, MoniePay
 */

using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class FundingCashierResponse
    {
        [JsonPropertyName("transactionId")]
        public Guid TransactionId { get; set; }

        [JsonPropertyName("cashierId")]
        public Guid CashierId { get; set; }

        [JsonPropertyName("TillAccountNumber")]
        public string TillAccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public Currency currency { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; } = default!;
    }
}
