using EventDriven.CQRS.Abstractions.Events;

namespace OrderService.Domain.OrderAggregate.Events
{

    using System;

    public record OrderCreated(Order Order) : DomainEvent<Guid>(Order.Id);
}
