namespace ESSample.EventConsumer.Infrastructure
{
    using ESSample.Infrastructure;
    using EventStore.ClientAPI;
    using Microsoft.Extensions.Logging;
    using Raven.Client.Documents;
    using System.Threading.Tasks;

    public class CheckpointRepository : ICheckpointRepository<Position>
    {
        private readonly DocumentStore _documentStore;
        private readonly ILogger<CheckpointRepository> _logger;

        public CheckpointRepository(DocumentStore documentStore, ILogger<CheckpointRepository> logger)
        {
            _documentStore = documentStore;
            _logger = logger;
        }

        private string GetDocumentId(string projection) => $"Checkpoints/{projection}";

        public async Task<Position> GetLastCheckpointAsync(string projection)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var id = GetDocumentId(projection);
                var document = await session.LoadAsync<CheckpointDocument>(id);

                if (document != null)
                {
                    return document.Checkpoint;
                }

                return default;
            }
        }

        public async Task SetLastCheckpointAsync(Position checkpoint, string projection)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var id = GetDocumentId(projection);
                var document = await session.LoadAsync<CheckpointDocument>(id);

                if (document != null)
                {
                    document.Checkpoint = checkpoint;
                }
                else
                {
                    await session.StoreAsync(new CheckpointDocument { Checkpoint = checkpoint }, id);
                }

                await session.SaveChangesAsync();
            }
        }
    }
}
