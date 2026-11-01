// SPDX-License-Identifier: Apache-2.0
/*
 * TransferVaultRequest made by cashier or customers
 * Copyright (c) 2026, MoniePay
 */
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoniePay.src.dto
{
    public class TransferToVaultRequest
    {
        [JsonPropertyName("bankAccountId")]
        [Required(ErrorMessage = "provide bankaccountId")]
        public Guid BankAccountId { get; set; }

        [JsonPropertyName("vaultId")]
        [Required(ErrorMessage = "provide vaultId")]
        public Guid VaultId { get; set; }

        [JsonPropertyName("amount")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.1")]
        public Guid Amount { get; set; }

        [JsonPropertyName("bankReference")]
        public required string BankingReference { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("idempotencyKey")]
        [Required(ErrorMessage = "provide bankaccountId")]
        public required string IdempotencyKey { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; } = string.Empty;
    }
}