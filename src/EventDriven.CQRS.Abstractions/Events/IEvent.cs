using System;

namespace EventDriven.CQRS.Abstractions.Events
{
    /// <summary>
    /// A statement of fact about what change has been made to the domain state.
    /// </summary>
    public interface IEvent<TId>
    {
        /// <summary>
        /// Unique ID of the event.
        /// </summary>
        TId Id { get; }

        /// <summary>
        /// Time at which the event was created.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// The name of the service that produced it and acts as a namespace.
        /// </summary>
        string Source { get; set; }
    }
}
