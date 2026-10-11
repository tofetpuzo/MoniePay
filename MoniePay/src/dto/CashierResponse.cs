// SPDX-License-Identifier: Apache-2.0
/*
 * Cashier Response: creates the cashier response after the admin funds the cashier
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */


using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class CashierResponse
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("username")]
        public Decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }


        public static CashierResponse From(CashierDTO cashierDTO) => new()
        {
            Email = cashierDTO.Email
            ,
            Amount = cashierDTO.Amount
            ,
            Currency = cashierDTO.Currency
            ,
            Status = cashierDTO.Status
            ,
            CreatedAt = DateTime.UtcNow

        };
    }
}
