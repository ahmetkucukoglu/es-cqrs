namespace ESSample.API.Controllers
{
    using Application.Meetup.Commands.AddComment;
    using Application.Meetup.Commands.AddPhoto;
    using Application.Meetup.Commands.CancelMeetup;
    using Application.Meetup.Commands.CompleteMeetup;
    using Application.Meetup.Commands.CreateMeetup;
    using Application.Meetup.Commands.JoinMeetup;
    using Application.Meetup.Commands.UpdateMeetup;
    using Application.Meetup.Queries.GetOpenMeetups;
    using ESSample.Application.Meetup.Queries.SuggestMeetup;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class MeetupsController : Controller
    {
        private readonly IMediator _mediator;

        public MeetupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("open")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var meetups = await _mediator.Send(new GetMeetupsQuery(), cancellationToken);

            return Ok(meetups);
        }

        [HttpGet]
        [Route("suggest")]
        public async Task<IActionResult> Get([FromBody] SuggestMeetupsQuery query, CancellationToken cancellationToken)
        {
            var meetups = await _mediator.Send(query, cancellationToken);

            return Ok(meetups);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Post(Guid id, [FromBody] CreateMeetupCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateMeetupCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route("{id}/join")]
        public async Task<IActionResult> Join(Guid id, [FromBody]JoinMeetupCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPatch]
        [Route("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id, [FromBody]CompleteMeetupCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPatch]
        [Route("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody]CancelMeetupCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/comments")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            var commentId = await _mediator.Send(command, cancellationToken);

            return Ok(commentId);
        }

        [HttpPost]
        [Route("{id}/photos")]
        public async Task<IActionResult> AddPhoto(Guid id, [FromBody] AddPhotoCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            var commentId = await _mediator.Send(command, cancellationToken);

            return Ok(commentId);
        }
    }
}
