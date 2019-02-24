namespace ESSample.EventConsumer
{
    using Application.Meetup.Projections;
    using Core;
    using ESSample.Infrastructure;
    using EventStore.ClientAPI;
    using Infrastructure;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.Elasticsearch;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .ConfigureHostConfiguration((configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configurationBuilder.AddJsonFile("appsettings.json");
                    configurationBuilder.AddEnvironmentVariables(prefix: "MEETUP_");
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection.AddMediatR(typeof(ProjectionNotification).GetTypeInfo().Assembly);

                    var domainEventMapper = new DomainEventMapper();
                    domainEventMapper.Map();

                    var documentStoreInitializer = new DocumentStoreInitializer(
                        url: hostBuilderContext.Configuration.GetValue<string>("RavenDB:Url"),
                        databaseName: hostBuilderContext.Configuration.GetValue<string>("RavenDB:DatabaseName"));

                    var documentStore = documentStoreInitializer.GetDocumentStore();

                    var elasticClientInitializer = new ElasticClientInitializer(
                        uri: hostBuilderContext.Configuration.GetValue<string>("ElasticSearch:Uri"));

                    var elasticClient = elasticClientInitializer.GetElasticClientAsync().GetAwaiter().GetResult();

                    var eventStoreConnectionInitializer = new EventStoreConnectionInitializer(
                        connectionString: hostBuilderContext.Configuration.GetValue<string>("EventStore:ConnectionString"),
                        connectionName: hostBuilderContext.Configuration.GetValue<string>("EventStore:ConnectionName"));

                    var eventStoreConnection = eventStoreConnectionInitializer.GetEventStoreConnectionAsync().GetAwaiter().GetResult();

                    serviceCollection.AddSingleton<Subscriber>();
                    serviceCollection.AddSingleton<ICheckpointRepository<Position>, CheckpointRepository>();
                    serviceCollection.AddSingleton<IDomainEventMapper>(domainEventMapper);
                    serviceCollection.AddSingleton(documentStore);
                    serviceCollection.AddSingleton(elasticClient);
                    serviceCollection.AddSingleton(eventStoreConnection);
                })
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
                {
                    Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostBuilderContext.Configuration.GetValue<string>("ElasticSearch:Uri")))
                    {
                        MinimumLogEventLevel = LogEventLevel.Verbose,
                        AutoRegisterTemplate = true
                    }).CreateLogger();

                    loggingBuilder.AddSerilog();
                }).Build();

            var subscriber = (Subscriber)host.Services.GetService(typeof(Subscriber));
            await subscriber.SubscribeAsync();

            await host.RunAsync();
        }
    }
}
