namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupDescriptionChanged : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public string Description { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
