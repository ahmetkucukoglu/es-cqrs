namespace ESSample.Application.Meetup.Commands.AddComment
{
    using Domain.MeetupAggregate;
    using ESSample.Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public AddCommentCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            aggregate.AddComment(request.CommentatorId, request.Comment);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
