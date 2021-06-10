using EventDriven.CQRS.Abstractions.Commands;

namespace CustomerService.Domain.CustomerAggregate.Commands
{

    using System;

    public record CreateCustomer(Customer Customer) : Command<Guid>.Create(Customer.Id);
}