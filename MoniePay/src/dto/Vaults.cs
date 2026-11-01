// SPDX-License-Identifier: Apache-2.0
/*
 * Vault model for customers and cash transfer
 * Copyright (c) 2026, MoniePay
 */

using MoniePay.src.models;

namespace MoniePay.src.dto
{
    public class Vaults
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public VaultType Type { get; set; }
        public string Location { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid CustodianId { get; set; }
        public Guid LedgerAccountId { get; set; }
        public LedgerAccounts? LedgerAccount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
