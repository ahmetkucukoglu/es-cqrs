namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupCompleted : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
