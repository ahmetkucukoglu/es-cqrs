namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupJoined : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public Guid ParticipantId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
