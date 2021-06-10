using EventDriven.CQRS.Abstractions.Events;

namespace CustomerService.Domain.CustomerAggregate.Events
{

    using System;

    public record CustomerCreated(Customer Customer) : DomainEvent<Guid>(Customer.Id);
}
