namespace ESSample.Application.Meetup.Commands.CompleteMeetup
{
    using MediatR;
    using System;

    public class CompleteMeetupCommand : IRequest
    {
        public Guid MeetupId { get; set; }
        public Guid OrganizerId { get; set; }
    }
}
