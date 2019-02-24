namespace ESSample.Core
{
    using ESSample.Domain;
    using ESSample.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DomainEventMapper : IDomainEventMapper
    {
        private readonly Dictionary<string, Type> _eventsByKey = new Dictionary<string, Type>();
        private readonly Dictionary<Type, string> _keysByEvent = new Dictionary<Type, string>();

        public string GetKey(Type @event)
        {
            _keysByEvent.TryGetValue(@event, out var key);

            return key;
        }

        public Type GetEvent(string key)
        {
            _eventsByKey.TryGetValue(key, out var @event);

            return @event;
        }

        public void Map()
        {
            var domainEvents = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where((x) => typeof(DomainEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (var domainEvent in domainEvents)
            {
                _eventsByKey.Add(domainEvent.Name, domainEvent);
                _keysByEvent.Add(domainEvent, domainEvent.Name);
            }
        }
    }
}
