namespace ESSample.API.Infrastructure
{
    using ESSample.Domain;
    using ESSample.Infrastructure;
    using EventStore.ClientAPI;
    using EventStore.ClientAPI.Exceptions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class AggregateRepository : IAggregateRepository
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IDomainEventMapper _domainEventMapper;
        private readonly ILogger<AggregateRepository> _logger;

        public AggregateRepository(
            IEventStoreConnection eventStoreConnection,
            IDomainEventMapper domainEventMapper,
            ILogger<AggregateRepository> logger)
        {
            _eventStoreConnection = eventStoreConnection;
            _domainEventMapper = domainEventMapper;
            _logger = logger;
        }

        private string GetStreamName<T>(T type, string aggregateId) => $"{typeof(T).Name}-{aggregateId}";

        private DomainEvent GetDomainEventByType(string type, byte[] data) => (DomainEvent)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), _domainEventMapper.GetEvent(type));

        private EventData GetEventDataByEvent(DomainEvent @event) =>
            new EventData(
                eventId: Guid.NewGuid(),
                type: _domainEventMapper.GetKey(@event.GetType()),
                isJson: true,
                data: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                metadata: null);

        public async Task<T> LoadAsync<T>(string aggregateId, CancellationToken cancellationToken = default) where T : AggregateRoot, new()
        {
            var aggregate = new T();
            var nextPageStart = 0L;

            var streamName = GetStreamName<T>(aggregate, aggregateId);

            do
            {
                var page = await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextPageStart, 4096, false, null);

                if (page.Events.Length > 0)
                {
                    var version = page.Events.Last().Event.EventNumber;

                    var histories = page.Events
                        .Select((@event) => GetDomainEventByType(@event.OriginalEvent.EventType, @event.OriginalEvent.Data))
                        .ToArray();

                    aggregate.LoadFromHistory(version, histories);
                }

                nextPageStart = !page.IsEndOfStream ? page.NextEventNumber : -1;

            } while (nextPageStart != -1);

            return aggregate;
        }

        public async Task SaveAsync<T>(T aggregate) where T : AggregateRoot, new()
        {
            var changes = aggregate.GetChanges()
                .Select((change) => GetEventDataByEvent(change))
                .ToArray();

            if (!changes.Any())
                return;

            var streamName = GetStreamName<T>(aggregate, aggregate.Id.ToString());

            try
            {
                await _eventStoreConnection.AppendToStreamAsync(streamName, aggregate.Version, changes, null);
            }
            catch (WrongExpectedVersionException)
            {
                var page = await _eventStoreConnection.ReadStreamEventsBackwardAsync(streamName, -1, 1, false, null);

                _logger.LogError("Version failed. Stream: {Stream}. Expected Version: {Version}. Last Version: {LastEventNumber}",
                    page.Stream, page.Status, aggregate.Version, page.LastEventNumber);

                throw new AggregateVersionFailedException(page.Stream, aggregate.Version, page.LastEventNumber);
            }
        }
    }
}
