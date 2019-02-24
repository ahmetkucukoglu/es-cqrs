namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupPhotoAdded : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public Guid PhotographerId { get; set; }
        public string PhotoPath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
