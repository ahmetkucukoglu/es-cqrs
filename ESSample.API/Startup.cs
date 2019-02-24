namespace ESSample.API
{
    using API.Domain.Policies;
    using API.Infrastructure;
    using Application.Meetup.Commands.CreateMeetup;
    using Core;
    using ESSample.Domain.MeetupAggregate.Policies;
    using ESSample.Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public class Startup
    {
        public IHostingEnvironment HostingEnvironment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateMeetupCommand).GetTypeInfo().Assembly);

            var domainEventMapper = new DomainEventMapper();
            domainEventMapper.Map();

            var documentStoreInitializer = new DocumentStoreInitializer(
                url: Configuration.GetValue<string>("RavenDB:Url"),
                databaseName: Configuration.GetValue<string>("RavenDB:DatabaseName"));

            var documentStore = documentStoreInitializer.GetDocumentStore();

            var elasticClientInitializer = new ElasticClientInitializer(
                uri: Configuration.GetValue<string>("ElasticSearch:Uri"));

            var elasticClient = elasticClientInitializer.GetElasticClientAsync().GetAwaiter().GetResult();

            var eventStoreConnectionInitializer = new EventStoreConnectionInitializer(
               connectionString: Configuration.GetValue<string>("EventStore:ConnectionString"),
               connectionName: Configuration.GetValue<string>("EventStore:ConnectionName"));

            var eventStoreConnection = eventStoreConnectionInitializer.GetEventStoreConnectionAsync().GetAwaiter().GetResult();

            services.AddSingleton(documentStore);
            services.AddSingleton(elasticClient);
            services.AddSingleton(eventStoreConnection);
            services.AddSingleton<IDomainEventMapper>(domainEventMapper);
            services.AddSingleton<IMeetupPolicy, MeetupPolicy>();
            services.AddSingleton<IAggregateRepository, AggregateRepository>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }
    }
}
