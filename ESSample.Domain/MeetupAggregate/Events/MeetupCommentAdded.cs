namespace ESSample.Domain.MeetupAggregate.Events
{
    using System;

    public class MeetupCommentAdded : DomainEvent
    {
        public Guid MeetupId { get; set; }
        public Guid CommentatorId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
