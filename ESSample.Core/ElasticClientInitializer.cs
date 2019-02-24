namespace ESSample.Core
{
    using Application.Meetup.Projections.AutoComplete.Models;
    using Nest;
    using System;
    using System.Threading.Tasks;

    public class ElasticClientInitializer
    {
        private readonly Uri _uri;

        public ElasticClientInitializer(string uri)
        {
            _uri = new Uri(uri);
        }

        private string GetSuggestIndex() => nameof(MeetupSuggest).ToLowerInvariant();

        public async Task<ElasticClient> GetElasticClientAsync()
        {
            var elasticClient = new ElasticClient(new ConnectionSettings(_uri));

            if (!elasticClient.IndexExists(GetSuggestIndex()).Exists)
            {
                var createIndexDescriptor = new CreateIndexDescriptor(nameof(MeetupSuggest).ToLowerInvariant())
                    .Mappings((ms) => ms.Map<MeetupSuggest>((m) => m.AutoMap().Properties((ps) => ps.Completion((c) => c.Name((p) => p.Suggest)))));

                var createIndexResponse = await elasticClient.CreateIndexAsync(createIndexDescriptor);

                if (createIndexResponse.IsValid)
                {
                    Console.WriteLine("Created elastic search index. IndexName: {0}.", GetSuggestIndex());
                }
            }

            return elasticClient;
        }
    }
}
