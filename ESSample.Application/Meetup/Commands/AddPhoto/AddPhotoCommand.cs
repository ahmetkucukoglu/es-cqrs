namespace ESSample.Application.Meetup.Commands.AddPhoto
{
    using MediatR;
    using System;

    public class AddPhotoCommand : IRequest
    {
        public Guid MeetupId { get; set; }
        public Guid PhotographerId { get; set; }
        public string Photo { get; set; }
    }
}
