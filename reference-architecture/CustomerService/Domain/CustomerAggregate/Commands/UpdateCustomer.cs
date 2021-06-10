using EventDriven.CQRS.Abstractions.Commands;

namespace CustomerService.Domain.CustomerAggregate.Commands
{

    using System;

    public record UpdateCustomer(Customer Customer) : Command<Guid>.Update(Customer.Id, Customer.ETag);
}