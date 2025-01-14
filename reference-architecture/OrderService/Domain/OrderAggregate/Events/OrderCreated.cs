﻿using EventDriven.CQRS.Abstractions.Events;

namespace OrderService.Domain.OrderAggregate.Events
{
    public record OrderCreated(Order Order) : DomainEvent(Order.Id);
}
