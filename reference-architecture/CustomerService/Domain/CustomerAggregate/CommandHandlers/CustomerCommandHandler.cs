using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Integration.Events;
using CustomerService.Domain.CustomerAggregate.Commands;
using CustomerService.Domain.CustomerAggregate.Events;
using CustomerService.Repositories;
using EventDriven.CQRS.Abstractions.Commands;
using EventDriven.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Integration = Common.Integration;

namespace CustomerService.Domain.CustomerAggregate.CommandHandlers
{

    using System;

    public class CustomerCommandHandler :
        ICommandHandler<Customer, Guid, CreateCustomer>,
        ICommandHandler<Customer, Guid, UpdateCustomer>,
        ICommandHandler<Customer, Guid, RemoveCustomer>
    {
        private readonly ICustomerRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerCommandHandler> _logger;

        public CustomerCommandHandler(
            ICustomerRepository repository,
            IEventBus eventBus,
            IMapper mapper,
            ILogger<CustomerCommandHandler> logger)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommandResult<Customer, Guid>> Handle(CreateCustomer command)
        {
            // Process command
            _logger.LogInformation("Handling command: {CommandName}", nameof(CreateCustomer));
            var events = command.Customer.Process(command);
            
            // Apply events
            var domainEvent = events.OfType<CustomerCreated>().SingleOrDefault();
            if (domainEvent == null) return new CommandResult<Customer, Guid>(CommandOutcome.NotHandled);
            command.Customer.Apply(domainEvent);
            
            // Persist entity
            var entity = await _repository.Add(command.Customer);
            if (entity == null) return new CommandResult<Customer, Guid>(CommandOutcome.InvalidCommand);
            return new CommandResult<Customer, Guid>(CommandOutcome.Accepted, entity);
        }

        public async Task<CommandResult<Customer, Guid>> Handle(UpdateCustomer command)
        {
            // Compare shipping addresses
            _logger.LogInformation("Handling command: {CommandName}", nameof(UpdateCustomer));
            var existing = await _repository.Get(command.EntityId);
            var addressChanged = command.Customer.ShippingAddress != existing.ShippingAddress;
            
            try
            {
                // Persist entity
                var entity = await _repository.Update(command.Customer);
                if (entity == null) return new CommandResult<Customer, Guid>(CommandOutcome.NotFound);
                
                // Publish events
                if (addressChanged)
                {
                    var shippingAddress = _mapper.Map<Integration.Models.Address>(entity.ShippingAddress);
                    _logger.LogInformation("Publishing event: {EventName}", $"v1.{nameof(CustomerAddressUpdated)}");
                    await _eventBus.PublishAsync(
                        new CustomerAddressUpdated(entity.Id, shippingAddress),
                        null, "v1");
                }
                return new CommandResult<Customer, Guid>(CommandOutcome.Accepted, entity);
            }
            catch (ConcurrencyException)
            {
                return new CommandResult<Customer, Guid>(CommandOutcome.Conflict);
            }
        }

        public async Task<CommandResult<Customer, Guid>> Handle(RemoveCustomer command)
        {
            // Persist entity
            _logger.LogInformation("Handling command: {CommandName}", nameof(RemoveCustomer));
            await _repository.Remove(command.EntityId);
            return new CommandResult<Customer, Guid>(CommandOutcome.Accepted);
        }
    }
}