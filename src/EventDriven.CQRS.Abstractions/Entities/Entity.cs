using System;

namespace EventDriven.CQRS.Abstractions.Entities
{
    /// <inheritdoc />
    public abstract class Entity<TId> : IEntity<TId>
    {
        /// <inheritdoc />
        public TId Id { get; set; }

        /// <inheritdoc />
        public long SequenceNumber { get; set; }

        /// <inheritdoc />
        public string ETag { get; set; }

        /// <inheritdoc />
        public EntityState State { get; set; } = EntityState.Active;

        /// <summary>
        /// Set entity state to Deleted.
        /// </summary>
        protected void MarkDeleted()
        {
            State = EntityState.Deleted;
        }

        /// <summary>
        /// Set entity state to Frozen.
        /// </summary>
        protected void Freeze()
        {
            State = EntityState.Frozen;
        }
    }
}