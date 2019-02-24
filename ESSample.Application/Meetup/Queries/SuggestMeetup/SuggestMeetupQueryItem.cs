namespace ESSample.Application.Meetup.Queries.SuggestMeetup
{
    using System;

    public class SuggestMeetupQueryItem
    {
        public Guid MeetupId { get; set; }
        public string Subject { get; set; }
        public double Score { get; set; }
    }
}
