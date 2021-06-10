using System.Threading.Tasks;

namespace EventDriven.CQRS.Abstractions.Events
{
    /// <inheritdoc />
    public abstract class EventHandler<TEvent, TId> : IEventHandler<TEvent, TId>
        where TEvent : class, IDomainEvent<TId>, IEvent<TId>
    {
        /// <inheritdoc />
        public abstract Task<bool> Handle(TEvent domainEvent);
    }
}
