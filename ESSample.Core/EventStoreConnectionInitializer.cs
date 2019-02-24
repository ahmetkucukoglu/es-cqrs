namespace ESSample.Core
{
    using EventStore.ClientAPI;
    using System;
    using System.Threading.Tasks;

    public class EventStoreConnectionInitializer
    {
        private readonly string _connectionString;
        private readonly string _connectionName;

        public EventStoreConnectionInitializer(string connectionString, string connectionName)
        {
            _connectionString = connectionString;
            _connectionName = connectionName;
        }

        public async Task<IEventStoreConnection> GetEventStoreConnectionAsync()
        {
            var eventStoreConnection = EventStoreConnection.Create(
                connectionString: _connectionString,
                builder: ConnectionSettings.Create().KeepReconnecting(),
                connectionName: _connectionName);

            eventStoreConnection.Connected += EventStoreConnection_Connected;

            await eventStoreConnection.ConnectAsync();

            return eventStoreConnection;
        }

        private void EventStoreConnection_Connected(object sender, ClientConnectionEventArgs e)
        {
            Console.WriteLine("Connected to event store. Remote Endpoint: {0}.", e.RemoteEndPoint);
        }
    }
}
