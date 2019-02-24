namespace ESSample.Application.Meetup.Commands.CreateMeetup
{
    using MediatR;
    using System;

    public class CreateMeetupCommand : IRequest
    {
        public Guid MeetupId { get; set; }
        public Guid OrganizerId { get; set; }
        public string Subject { get; set; }
        public DateTime When { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
