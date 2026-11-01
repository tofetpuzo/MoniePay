// SPDX-License-Identifier: Apache-2.0
/*
 * Banking Funding Response model for customers and cash transfer
 * Copyright (c) 2026, MoniePay
 */
using MoniePay.src.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class BankFundingsResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("bankAccountId")]
        [Required(ErrorMessage = "provide bankaccountId")]
        public Guid BankAccountId { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("amount")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.1")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("bankReference")]
        public required string BankReference { get; set; } = string.Empty;

        [JsonPropertyName("transactionId")]
        public Guid? TransactionId { get; set; }

        [JsonPropertyName("idempotencyKey")]
        [Required(ErrorMessage = "provide bankaccountId")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("narration")]
        public DateTime? ConfirmedAt { get; set; }

        public static BankFundingsResponse From(BankFundings f) => new()
        {
            Id = f.Id,
            BankAccountId = f.BankAccountId,
            Status = f.Status,
            Amount = f.Amount,
            Currency = f.Currency,
            BankReference = f.BankReference,
            TransactionId = f.TransactionId,
            CreatedAt = f.CreatedAt,
            ConfirmedAt = f.ConfirmedAt,
        };
    }
}
