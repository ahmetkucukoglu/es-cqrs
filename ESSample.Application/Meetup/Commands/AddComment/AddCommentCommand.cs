namespace ESSample.Application.Meetup.Commands.AddComment
{
    using MediatR;
    using System;

    public class AddCommentCommand : IRequest
    {
        public Guid MeetupId { get; set; }
        public Guid CommentatorId { get; set; }
        public string Comment { get; set; }
    }
}
