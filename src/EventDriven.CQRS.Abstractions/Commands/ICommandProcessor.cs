using EventDriven.CQRS.Abstractions.Events;
using System.Collections.Generic;

namespace EventDriven.CQRS.Abstractions.Commands
{

    /// <summary>
    /// Processes a command by generating one or more domain events.
    /// </summary>
    /// <typeparam name="TCommand">The type of command</typeparam>
    /// <typeparam name="TEntityId"></typeparam>
    public interface ICommandProcessor<in TCommand, TEntityId> where TCommand : class, ICommand<TEntityId>
    {
        /// <summary>
        /// Process specified command by creating one or more domain events.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>Domain events resulting from the command.</returns>
        IEnumerable<IDomainEvent<TEntityId>> Process(TCommand command);
    }
}
