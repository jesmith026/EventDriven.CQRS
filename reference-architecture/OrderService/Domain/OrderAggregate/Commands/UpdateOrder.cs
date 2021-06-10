using EventDriven.CQRS.Abstractions.Commands;

namespace OrderService.Domain.OrderAggregate.Commands
{

    using System;

    public record UpdateOrder(Order Order) : Command<Guid>.Update(Order.Id, Order.ETag);
}