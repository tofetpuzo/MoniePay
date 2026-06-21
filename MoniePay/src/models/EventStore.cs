// SPDX-License-Identifier: Apache-2.0
/*
 * Append-only domain event log.
 *
 * Copyright (c) 2026, MoniePay
 */

using System.Text.Json;

namespace MoniePay.src.models
{
    public class EventStore
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public required JsonDocument Payload { get; set; }
        public DateTime CreateAt { get; set; }
        public EventStore() { }

        public EventStore(Guid id, Guid aggregateId, string eventType, JsonDocument payload, DateTime createdAt)
        {
            Id = id;
            AggregateId = aggregateId;
            EventType = eventType;
            Payload = payload;
            CreateAt = createdAt;
        }
    }
}
