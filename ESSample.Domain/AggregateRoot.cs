namespace ESSample.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AggregateRoot
    {
        readonly IList<DomainEvent> _changes = new List<DomainEvent>();

        public Guid Id { get; protected set; } = Guid.Empty;
        public long Version { get; private set; } = -1;

        protected abstract void DoApply(DomainEvent @event);

        public void ApplyEvent(DomainEvent @event)
        {
            DoApply(@event);

            _changes.Add(@event);
        }

        public void LoadFromHistory(long version, IEnumerable<DomainEvent> history)
        {
            Version = version;

            foreach (var @event in history)
            {
                DoApply(@event);
            }
        }

        public DomainEvent[] GetChanges() => _changes.ToArray();
    }
}
