namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupWhenChanged : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public DateTime When { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
