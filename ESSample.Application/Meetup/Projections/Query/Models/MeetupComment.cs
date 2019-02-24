namespace ESSample.Application.Meetup.Projections.Query.Models
{
    using System;

    public class MeetupComment
    {
        public Guid CommentatorId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
