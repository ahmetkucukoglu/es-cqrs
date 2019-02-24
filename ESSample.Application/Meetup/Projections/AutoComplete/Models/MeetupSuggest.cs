namespace ESSample.Application.Meetup.Projections.AutoComplete.Models
{
    using Nest;
    using System;

    public class MeetupSuggest
    {
        public Guid MeetupId { get; set; }
        public string Subject { get; set; }
        public CompletionField Suggest { get; set; }
    }
}
