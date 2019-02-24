namespace ESSample.Application.Meetup.Commands.JoinMeetup
{
    using Domain.MeetupAggregate;
    using ESSample.Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class JoinMeetupCommandHandler : IRequestHandler<JoinMeetupCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public JoinMeetupCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(JoinMeetupCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            aggregate.Join(request.ParticipantId);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
