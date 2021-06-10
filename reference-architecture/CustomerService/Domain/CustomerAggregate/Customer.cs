using System;
using System.Collections.Generic;
using CustomerService.Domain.CustomerAggregate.Commands;
using CustomerService.Domain.CustomerAggregate.Events;
using EventDriven.CQRS.Abstractions.Commands;
using EventDriven.CQRS.Abstractions.Entities;
using EventDriven.CQRS.Abstractions.Events;

namespace CustomerService.Domain.CustomerAggregate
{
    public class Customer : 
        Entity<Guid>,
        ICommandProcessor<CreateCustomer, Guid>,
        IEventApplier<CustomerCreated, Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address ShippingAddress { get; set; }

        public IEnumerable<IDomainEvent<Guid>> Process(CreateCustomer command)
            // To process command, return one or more domain events
            => new List<IDomainEvent<Guid>>
            {
                new CustomerCreated(command.Customer)
            };

        public void Apply(CustomerCreated domainEvent) =>
            // Set Id
            Id = domainEvent.EntityId != default(Guid) ? domainEvent.EntityId : Guid.NewGuid();
    }
}