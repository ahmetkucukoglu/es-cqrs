namespace ESSample.EventConsumer.Infrastructure
{
    using Application.Meetup.Projections;
    using Domain;
    using ESSample.Infrastructure;
    using EventStore.ClientAPI;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class Subscriber
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly ICheckpointRepository<Position> _checkpointRepository;
        private readonly ILogger<Subscriber> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IDomainEventMapper _domainEventMapper;

        public Subscriber(
            IEventStoreConnection eventStoreConnection,
            ICheckpointRepository<Position> checkpointRepository,
            ILogger<Subscriber> logger,
            IConfiguration configuration,
            IMediator mediator,
            IDomainEventMapper domainEventMapper)
        {
            _eventStoreConnection = eventStoreConnection;
            _checkpointRepository = checkpointRepository;
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
            _domainEventMapper = domainEventMapper;
        }

        private string GetProjectionName() => _configuration.GetValue<string>("CheckpointStore:ProjectionName");

        private DomainEvent GetDomainEventByType(string type, byte[] data) => (DomainEvent)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), _domainEventMapper.GetEvent(type));

        public async Task SubscribeAsync()
        {
            var lastCheckpoint = await _checkpointRepository.GetLastCheckpointAsync(GetProjectionName());

            var settings = new CatchUpSubscriptionSettings(
                maxLiveQueueSize: 10000,
                readBatchSize: 500,
                verboseLogging: false,
                resolveLinkTos: false,
                subscriptionName: "Meetup");

            _eventStoreConnection.SubscribeToAllFrom(
                lastCheckpoint: lastCheckpoint,
                settings: settings,
                eventAppeared: async (sub, @event) =>
                {
                    if (@event.OriginalEvent.EventType.StartsWith("$"))
                        return;

                    try
                    {
                        await _mediator.Publish(new ProjectionNotification
                        {
                            DomainEvent = GetDomainEventByType(@event.OriginalEvent.EventType, @event.OriginalEvent.Data)
                        });

                        await _checkpointRepository.SetLastCheckpointAsync(@event.OriginalPosition.Value, GetProjectionName());
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, exception.Message);
                    }
                },
                liveProcessingStarted: (sub) =>
                {
                    _logger.LogInformation("{SubscriptionName} subscription started.", sub.SubscriptionName);
                },
                subscriptionDropped: (sub, subDropReason, exception) =>
                {
                    _logger.LogWarning("{SubscriptionName} dropped. Reason: {SubDropReason}.", sub.SubscriptionName, subDropReason);
                });
        }
    }
}
