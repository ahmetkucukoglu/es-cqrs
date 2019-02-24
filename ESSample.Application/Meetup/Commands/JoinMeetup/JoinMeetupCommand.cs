namespace ESSample.Application.Meetup.Commands.JoinMeetup
{
    using MediatR;
    using System;

    public class JoinMeetupCommand : IRequest
    {
        public Guid MeetupId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
