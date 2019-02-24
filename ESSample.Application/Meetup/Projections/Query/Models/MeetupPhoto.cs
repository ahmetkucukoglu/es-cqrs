namespace ESSample.Application.Meetup.Projections.Query.Models
{
    using System;

    public class MeetupPhoto
    {
        public Guid PhotographerId { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
