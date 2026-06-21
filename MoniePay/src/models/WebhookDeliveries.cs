// SPDX-License-Identifier: Apache-2.0
/*
 * Outbound webhook delivery attempts and outcomes.
 *
 * Copyright (c) 2026, MoniePay
 */

namespace MoniePay.src.models
{
    public class WebhookDeliveries
    {
        public Guid Id { get; set; }
        public Guid WebhookId { get; set; }
        public Guid EventId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public DateTime LastAttemptAt { get; set; }
        public WebhookDeliveries() { }
        public WebhookDeliveries(Guid id, Guid webhookId, Guid eventId,
            string status, int retryCount, DateTime lastAttempt)
        {
            Id = id;
            WebhookId = webhookId;
            EventId = eventId;
            Status = status;
            RetryCount = retryCount;
        }
    }
}