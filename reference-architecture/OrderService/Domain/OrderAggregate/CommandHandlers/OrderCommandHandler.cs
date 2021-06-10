using System.Linq;
using System.Threading.Tasks;
using EventDriven.CQRS.Abstractions.Commands;
using Microsoft.Extensions.Logging;
using OrderService.Domain.OrderAggregate.Commands;
using OrderService.Domain.OrderAggregate.Events;
using OrderService.Repositories;

namespace OrderService.Domain.OrderAggregate.CommandHandlers
{

    using System;

    public class OrderCommandHandler :
        ICommandHandler<Order, Guid, CreateOrder>,
        ICommandHandler<Order, Guid, UpdateOrder>,
        ICommandHandler<Order, Guid, RemoveOrder>,
        ICommandHandler<Order, Guid, ShipOrder>,
        ICommandHandler<Order, Guid, CancelOrder>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderCommandHandler> _logger;

        public OrderCommandHandler(
            IOrderRepository repository,
            ILogger<OrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CommandResult<Order, Guid>> Handle(CreateOrder command)
        {
            // Process command
            _logger.LogInformation("Handling command: {CommandName}", nameof(CreateOrder));
            var events = command.Order.Process(command);
            
            // Apply events
            var domainEvent = events.OfType<OrderCreated>().SingleOrDefault();
            if (domainEvent == null) return new CommandResult<Order, Guid>(CommandOutcome.NotHandled);
            command.Order.Apply(domainEvent);
            
            // Persist entity
            var entity = await _repository.AddOrder(command.Order);
            if (entity == null) return new CommandResult<Order, Guid>(CommandOutcome.InvalidCommand);
            return new CommandResult<Order, Guid>(CommandOutcome.Accepted, entity);
        }

        public async Task<CommandResult<Order, Guid>> Handle(UpdateOrder command)
        {
            _logger.LogInformation("Handling command: {CommandName}", nameof(UpdateOrder));

            try
            {
                // Persist entity
                var entity = await _repository.UpdateOrder(command.Order);
                if (entity == null) return new CommandResult<Order, Guid>(CommandOutcome.NotFound);
                return new CommandResult<Order, Guid>(CommandOutcome.Accepted, entity);
            }
            catch (ConcurrencyException)
            {
                return new CommandResult<Order, Guid>(CommandOutcome.Conflict);
            }
        }

        public async Task<CommandResult<Order, Guid>> Handle(RemoveOrder command)
        {
            // Persist entity
            _logger.LogInformation("Handling command: {CommandName}", nameof(RemoveOrder));
            await _repository.RemoveOrder(command.EntityId);
            return new CommandResult<Order, Guid>(CommandOutcome.Accepted);
        }

        public async Task<CommandResult<Order, Guid>> Handle(ShipOrder command)
        {
            // Process command
            _logger.LogInformation("Handling command: {CommandName}", nameof(ShipOrder));
            var entity = await _repository.GetOrder(command.EntityId);
            if (entity == null) return new CommandResult<Order, Guid>(CommandOutcome.NotFound);
            var events = entity.Process(command);
            
            // Apply events
            var domainEvent = events.OfType<OrderShipped>().SingleOrDefault();
            if (domainEvent == null) return new CommandResult<Order, Guid>(CommandOutcome.NotHandled);
            entity.Apply(domainEvent);
            
            try
            {
                // Persist entity
                var order = await _repository.UpdateOrderState(entity, OrderState.Shipped);
                if (order == null) return new CommandResult<Order, Guid>(CommandOutcome.NotFound);
                return new CommandResult<Order, Guid>(CommandOutcome.Accepted, order);
            }
            catch (ConcurrencyException)
            {
                return new CommandResult<Order, Guid>(CommandOutcome.Conflict);
            }
        }

        public async Task<CommandResult<Order, Guid>> Handle(CancelOrder command)
        {
            // Process command
            _logger.LogInformation("Handling command: {CommandName}", nameof(CancelOrder));
            var entity = await _repository.GetOrder(command.EntityId);
            if (entity == null) return new CommandResult<Order, Guid>(CommandOutcome.NotFound);
            var events = entity.Process(command);
            
            // Apply events
            var domainEvent = events.OfType<OrderCancelled>().SingleOrDefault();
            if (domainEvent == null) return new CommandResult<Order, Guid>(CommandOutcome.NotHandled);
            entity.Apply(domainEvent);
            
            try
            {
                // Persist entity
                var order = await _repository.UpdateOrderState(entity, OrderState.Cancelled);
                if (order == null) return new CommandResult<Order, Guid>(CommandOutcome.NotFound);
                return new CommandResult<Order, Guid>(CommandOutcome.Accepted, order);
            }
            catch (ConcurrencyException)
            {
                return new CommandResult<Order, Guid>(CommandOutcome.Conflict);
            }
        }
    }
}