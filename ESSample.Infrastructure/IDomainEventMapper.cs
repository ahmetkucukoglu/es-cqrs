namespace ESSample.Infrastructure
{
    using System;

    public interface IDomainEventMapper
    {
        void Map();
        Type GetEvent(string key);
        string GetKey(Type @event);
    }
}
