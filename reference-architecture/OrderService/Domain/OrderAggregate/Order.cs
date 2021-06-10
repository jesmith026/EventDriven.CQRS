using System;
using System.Collections.Generic;
using EventDriven.CQRS.Abstractions.Commands;
using EventDriven.CQRS.Abstractions.Entities;
using EventDriven.CQRS.Abstractions.Events;
using OrderService.Domain.OrderAggregate.Commands;
using OrderService.Domain.OrderAggregate.Events;

namespace OrderService.Domain.OrderAggregate
{
    public class Order : 
        Entity<Guid>,
        ICommandProcessor<CreateOrder, Guid>,
        IEventApplier<OrderCreated, Guid>,
        ICommandProcessor<ShipOrder, Guid>,
        IEventApplier<OrderShipped, Guid>,
        ICommandProcessor<CancelOrder, Guid>,
        IEventApplier<OrderCancelled, Guid>
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Address ShippingAddress { get; set; }
        public OrderState OrderState { get; set; }

        public IEnumerable<IDomainEvent<Guid>> Process(CreateOrder command)
            // To process command, return one or more domain events
            => new List<IDomainEvent<Guid>>
            {
                new OrderCreated(command.Order)
            };

        public void Apply(OrderCreated domainEvent) =>
            // Set Id
            Id = domainEvent.EntityId != default(Guid) ? domainEvent.EntityId : Guid.NewGuid();

        public IEnumerable<IDomainEvent<Guid>> Process(ShipOrder command)
            // To process command, return one or more domain events
            => new List<IDomainEvent<Guid>>
            {
                new OrderShipped(command.EntityId, command.ETag)
            };

        public void Apply(OrderShipped domainEvent)
        {
            // To apply events, mutate the entity state
            OrderState = OrderState.Shipped;
            ETag = domainEvent.ETag;
        }

        public IEnumerable<IDomainEvent<Guid>> Process(CancelOrder command)
            // To process command, return one or more domain events
            => new List<IDomainEvent<Guid>>
            {
                new OrderCancelled(command.EntityId, command.EntityEtag)
            };

        public void Apply(OrderCancelled domainEvent)
        {
            // To apply events, mutate the entity state
            OrderState = OrderState.Cancelled;
            ETag = domainEvent.ETag;
        }
    }
}