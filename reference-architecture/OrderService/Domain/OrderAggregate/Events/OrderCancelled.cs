﻿using System;
using EventDriven.CQRS.Abstractions.Events;

namespace OrderService.Domain.OrderAggregate.Events
{
    public record OrderCancelled(Guid EntityId, string ETag) : DomainEvent<Guid>(EntityId, ETag);
}
