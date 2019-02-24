namespace ESSample.Application.Meetup.Commands.CancelMeetup
{
    using MediatR;
    using System;

    public class CancelMeetupCommand : IRequest
    {
        public Guid MeetupId { get; set; }
        public Guid OrganizerId { get; set; }
    }
}
