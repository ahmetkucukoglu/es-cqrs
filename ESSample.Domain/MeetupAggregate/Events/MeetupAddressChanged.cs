namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupAddressChanged : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public string Address { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
