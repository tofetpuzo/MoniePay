// SPDX-License-Identifier: Apache-2.0
/*
 * Realized payment recorded with the upstream provider.
 *
 * Copyright (c) 2026, MoniePay
 */

namespace MoniePay.src.models
{
    public class Payments
    {
        public Guid Id { get; set; }
        public Guid PaymentIntentId { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string ProviderReference { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.PROCESSING;
        public Channel Channel { get; set; } = Channel.API;
        public string Attempt { get; set; } = string.Empty;
        public bool IsFinal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PaymentAttempts? PaymentAttempts { get; set; }
        public Payments() { }
        public Payments(Guid id, Guid paymentIntentId, string provider, string providerReference,
            Status status, string attempt, bool isFinal, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PaymentIntentId = paymentIntentId;
            Provider = provider;
            ProviderReference = providerReference;
            Status = status;
            Attempt = attempt;
            IsFinal = isFinal;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
