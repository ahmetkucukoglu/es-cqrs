namespace ESSample.Application.Meetup.Queries.GetOpenMeetups
{
    using MediatR;
    using Projections.Query.Models;
    using Raven.Client.Documents;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetMeetupsQueryHandler : IRequestHandler<GetMeetupsQuery, IEnumerable<GetMeetupQueryItem>>
    {
        private readonly DocumentStore _documentStore;

        public GetMeetupsQueryHandler(DocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<IEnumerable<GetMeetupQueryItem>> Handle(GetMeetupsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<GetMeetupQueryItem> result = null;

            using (var session = _documentStore.OpenAsyncSession())
            {
                result = await session.Query<MeetupDocument>()
                    .Where((x) => !x.Cancelled && !x.Completed)
                    .OrderByDescending((x) => x.When)
                    .Select((x) => new GetMeetupQueryItem
                    {
                        Address = x.Location.Address,
                        Description = x.Description,
                        Subject = x.Subject,
                        When = x.When
                    })
                    .ToListAsync(cancellationToken);
            }

            return result;
        }

        #region Helpers

        private GetMeetupQueryItem MapMeetup(MeetupDocument document)
        {
            var meetupDetail = new GetMeetupQueryItem
            {
                Address = document.Location.Address,
                Description = document.Description,
                Subject = document.Subject,
                When = document.When
            };

            return meetupDetail;
        }

        #endregion
    }
}
