namespace ESSample.Application.Meetup.Queries.SuggestMeetup
{
    using MediatR;
    using Nest;
    using Projections.AutoComplete.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SuggestMeetupsQueryHandler : IRequestHandler<SuggestMeetupsQuery, IEnumerable<SuggestMeetupQueryItem>>
    {
        private readonly ElasticClient _elasticClient;

        public SuggestMeetupsQueryHandler(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        private string GetSuggestIndex() => nameof(MeetupSuggest).ToLowerInvariant();

        public async Task<IEnumerable<SuggestMeetupQueryItem>> Handle(SuggestMeetupsQuery request, CancellationToken cancellationToken)
        {
            ISearchResponse<MeetupSuggest> searchResponse = await _elasticClient.SearchAsync<MeetupSuggest>((s) => s
                                     .Index(GetSuggestIndex())
                                     .Suggest((su) => su
                                          .Completion("suggestions", (c) => c
                                               .Field((f) => f.Suggest)
                                               .Prefix(request.Term)
                                               .Fuzzy((f) => f
                                                   .Fuzziness(Fuzziness.Auto))
                                               .Size(10))));

            var suggests = from suggest in searchResponse.Suggest["suggestions"]
                           from option in suggest.Options
                           select MapMeetup(option);
        
            return suggests;
        }

        #region Helpers

        private SuggestMeetupQueryItem MapMeetup(SuggestOption<MeetupSuggest> document)
        {
            var meetupDetail = new SuggestMeetupQueryItem
            {
                MeetupId = document.Source.MeetupId,
                Subject = document.Source.Subject,
                Score = document.Score
            };

            return meetupDetail;
        }

        #endregion
    }
}
