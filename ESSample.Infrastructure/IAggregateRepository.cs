namespace ESSample.Infrastructure
{
    using Domain;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAggregateRepository
    {
        Task<T> LoadAsync<T>(string aggregateId, CancellationToken cancellationToken = default) where T : AggregateRoot, new();
        Task SaveAsync<T>(T aggregate) where T : AggregateRoot, new();
    }
}
