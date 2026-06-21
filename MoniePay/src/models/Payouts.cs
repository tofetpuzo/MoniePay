// SPDX-License-Identifier: Apache-2.0
/*
 * Settlement records to merchants.
 *
 * Copyright (c) 2026, MoniePay
 */

namespace MoniePay.src.models
{
    public class Payouts
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Payouts() { }

        public Payouts(Guid id, Guid merchantId, Decimal amount, string status, DateTime createdAt)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            Status = status;
            CreatedAt = createdAt;
        }
    }
}
