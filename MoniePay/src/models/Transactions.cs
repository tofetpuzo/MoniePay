// SPDX-License-Identifier: Apache-2.0
/*
 * Recorded financial transactions tied to a payment intent.
 *
 * Copyright (c) 2026, MoniePay
 */

namespace MoniePay.src.models
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public Guid PaymentIntentId { get; set; }
        public Status Status { get; set; } = Status.PROCESSING;
        public DateTime CreatedAt { get; set; }
        public ICollection<LedgerEntries> LedgerEntries { get; set; } = new List<LedgerEntries>();

        public Transactions() { }

        public Transactions(Guid id, Guid paymentIntentId, DateTime createdAt)
        {
            Id = id;
            PaymentIntentId = paymentIntentId;
            CreatedAt = createdAt;
        }
    }
}
