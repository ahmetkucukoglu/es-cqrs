namespace ESSample.Application.Meetup.Projections.AutoComplete
{
    using Domain.MeetupAggregate.Events;
    using MediatR;
    using Nest;
    using Projections.AutoComplete.Models;
    using System.Threading;
    using System.Threading.Tasks;

    public class AutoCompleteProjectionHandler : INotificationHandler<ProjectionNotification>
    {
        private readonly ElasticClient _elasticClient;

        public AutoCompleteProjectionHandler(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        private bool IsSupported(object e)
            => e is MeetupRegistered
            || e is MeetupSubjectChanged;

        private string GetSuggestIndex() => nameof(MeetupSuggest).ToLowerInvariant();

        public async Task Handle(ProjectionNotification notification, CancellationToken cancellationToken)
        {
            if (!IsSupported(notification.DomainEvent))
                return;

            switch (notification.DomainEvent)
            {
                case MeetupRegistered x:
                    await OnRegistered(x);
                    break;
                case MeetupSubjectChanged x:
                    await OnSubjectChanged(x);
                    break;
            }
        }

        private async Task OnRegistered(MeetupRegistered @event)
        {
            var meetupSuggest = new MeetupSuggest
            {
                MeetupId = @event.MeetupId,
                Subject = @event.Subject,
                Suggest = new CompletionField
                {
                    Input = new[] { @event.Subject }
                }
            };

            await _elasticClient.IndexAsync<MeetupSuggest>(meetupSuggest, (u) => u.Index(GetSuggestIndex()).Id(meetupSuggest.MeetupId));
        }

        private async Task OnSubjectChanged(MeetupSubjectChanged @event)
        {
            var meetupSuggest = new MeetupSuggest
            {
                MeetupId = @event.MeetupId,
                Subject = @event.Subject,
                Suggest = new CompletionField
                {
                    Input = new[] { @event.Subject }
                }
            };

            await _elasticClient.UpdateAsync<MeetupSuggest, object>(@event.MeetupId, (u) => u.Index(GetSuggestIndex()).Doc(meetupSuggest));
        }
    }
}
