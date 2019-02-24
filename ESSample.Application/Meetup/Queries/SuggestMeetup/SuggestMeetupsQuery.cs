namespace ESSample.Application.Meetup.Queries.SuggestMeetup
{
    using MediatR;
    using System.Collections.Generic;

    public class SuggestMeetupsQuery : IRequest<IEnumerable<SuggestMeetupQueryItem>>
    {
        public string Term { get; set; }
    }
}
