using System.Threading.Tasks;

namespace EventDriven.CQRS.Abstractions.Events
{

    /// <summary>
    /// Event handler.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IEventHandler<in TEvent, TId> where TEvent : class, IDomainEvent<TId>, IEvent<TId>
    {
        /// <summary>
        /// Handles an event.
        /// </summary>
        /// <param name="domainEvent">The event.</param>
        /// <returns>True if handled successfully.</returns>
        Task<bool> Handle(TEvent domainEvent);
    }
}
