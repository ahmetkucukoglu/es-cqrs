namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupRegistered : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public Guid OrganizerId { get; set; }
        public string Subject { get; set; }
        public DateTime When { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
