namespace ESSample.API.Domain.Policies
{
    using Application.Meetup.Projections.Query.Models;
    using ESSample.Domain.MeetupAggregate;
    using ESSample.Domain.MeetupAggregate.Policies;
    using Raven.Client.Documents;
    using System;
    using System.Threading.Tasks;

    public class MeetupPolicy : IMeetupPolicy
    {
        private readonly DocumentStore _documentStore;

        public MeetupPolicy(DocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task CheckCanDefineMeetupAsync(OrganizerId organizerId, DateTime when)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var meetupCount = await session.Query<MeetupDocument>()
                    .CountAsync((x) => !x.Cancelled && x.OrganizerId == organizerId && x.When.Date == when.Date);

                if (meetupCount > 0)
                {
                    throw new MeetupDomainException("A maximum of one meetup is defined");
                }
            }
        }
    }
}
