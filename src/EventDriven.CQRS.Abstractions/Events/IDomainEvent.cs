using System;

namespace EventDriven.CQRS.Abstractions.Events
{
    /// <inheritdoc />
    public interface IDomainEvent<TId> : IEvent<TId>
    {
        /// <summary>
        /// The id of the entity that this event is "about".
        /// </summary>
        /// <value></value>
        TId EntityId { get; }

        /// <summary>
        /// Indicates this is the nth event related to a specific EntityId.
        /// </summary>
        long EntitySequenceNumber { get; }
    }
}