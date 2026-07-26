// SPDX-License-Identifier: Apache-2.0
/*
 * Payment intent aggregate root with idempotency key.
 *
 * Copyright (c) 2026, MoniePay
 */
namespace MoniePay.src.models

{
    public class PaymentIntents
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public Status status { get; set; } = Status.PROCESSING;
        public Channel channel { get; set; } = Channel.API;
        public string IdempotencyKey { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Customer? Customer { get; set; }
        public Payments? Payment { get; set; }
        public Transactions? Transaction { get; set; }
        public Payouts? Payout { get; set; }
        public PaymentAttempts? PaymentAttempts { get; set; }

        public PaymentIntents() { }

        public PaymentIntents(Guid id, Guid customerId, decimal amount, string currency, Status status, Channel channel, string idempotencyKey, string reference, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CustomerId = customerId;
            Amount = amount;
            Currency = currency;
            status = this.status;
            channel = this.channel;
            IdempotencyKey = idempotencyKey;
            Reference = reference;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

    }
}
