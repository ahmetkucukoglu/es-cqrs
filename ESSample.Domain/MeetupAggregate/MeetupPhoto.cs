namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class MeetupPhoto
    {
        public PhotographerId PhotographerId { get; set; }
        public Photo Photo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
