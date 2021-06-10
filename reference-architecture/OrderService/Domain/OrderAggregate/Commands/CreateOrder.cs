using EventDriven.CQRS.Abstractions.Commands;

namespace OrderService.Domain.OrderAggregate.Commands
{

    using System;

    public record CreateOrder(Order Order) : Command<Guid>.Create(Order.Id);
}