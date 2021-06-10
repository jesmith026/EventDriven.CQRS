using System.Threading.Tasks;
using EventDriven.CQRS.Abstractions.Entities;

namespace EventDriven.CQRS.Abstractions.Commands
{

    /// <summary>
    /// Command handler.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// /// <typeparam name="TEntityId"></typeparam>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    public interface ICommandHandler<TEntity, TEntityId, in TCommand>
        where TEntity : IEntity<TEntityId>
        where TCommand : class, ICommand<TEntityId>
    {
        /// <summary>
        /// Handles a command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The command result.</returns>
        Task<CommandResult<TEntity, TEntityId>> Handle(TCommand command);
    }
}
