namespace ESSample.EventConsumer.Infrastructure
{
    using EventStore.ClientAPI;

    public class CheckpointDocument
    {
        public Position Checkpoint { get; set; }
    }
}
