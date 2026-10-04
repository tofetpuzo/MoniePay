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
    }
}
