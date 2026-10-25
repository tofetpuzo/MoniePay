// SPDX-License-Identifier: Apache-2.0
/*
 * FundingCashierRequest: funding for moniepay
 * Copyright (c) 2026, MoniePay
 */

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class FundingCashierRequest
    {
        [JsonPropertyName("First Name")]
        [Required(ErrorMessage = "first name is required")]
        public Guid CashierId { get; set; }

        [JsonPropertyName("amount")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.1")]
        public required decimal Amount { get; set; }

        [JsonPropertyName("Currency")]
        [Required(ErrorMessage = "Currency is required")]
        public required Currency currency { get; set; }

        public Channel channel { get; set; } = Channel.COUNTER;

        [JsonPropertyName("idempotencyKey")]
        [Required(ErrorMessage = "idempotencyKey is required")]
        public required string IdempotencyKey { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;
    }
}
