namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupSubjectChanged : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public string Subject { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
