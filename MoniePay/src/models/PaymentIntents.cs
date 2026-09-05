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
        public Channel channel { get; set; } = Channel.API;
        public Status Status { get; set; } = Status.PROCESSING;
        public string IdempotencyKey { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Customer? Customer { get; set; }
        public string DestinationAccountNumber { get; set; } = string.Empty;
        public string DestinationAccountName { get; set; } = string.Empty;
        public Payments? Payment { get; set; }
        public Transactions? Transaction { get; set; }
        public Payouts? Payout { get; set; }
        public PaymentAttempts? PaymentAttempts { get; set; }

        public PaymentIntents() { }

        // Payment is deliberately not a constructor parameter. An intent exists
        // before any payment does; PaymentService.SettleIntent creates the
        // Payments row and assigns it afterwards.
        public PaymentIntents(Guid id, Guid customerId, decimal amount, string currency, string idempotencyKey,
            string reference, DateTime createdAt, DateTime updatedAt)
        {

            if (amount <= 0)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(nameof(amount), "add more money");
            }

            Id = id;
            CustomerId = customerId;
            Amount = amount;
            Currency = currency;
            IdempotencyKey = idempotencyKey;
            Reference = reference;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
