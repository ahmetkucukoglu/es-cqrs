namespace ESSample.Infrastructure
{
    using System.Threading.Tasks;

    public interface ICheckpointRepository<T>
    {
        Task<T> GetLastCheckpointAsync(string projection);
        Task SetLastCheckpointAsync(T checkpoint, string projection);
    }
}
