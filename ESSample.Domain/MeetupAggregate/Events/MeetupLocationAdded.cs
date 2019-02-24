namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupLocationAdded : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
