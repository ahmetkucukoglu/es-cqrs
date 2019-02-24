namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class MeetupComment
    {
        public CommentatorId CommentatorId { get; set; }
        public Comment Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
