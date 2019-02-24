namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupCancelled : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
