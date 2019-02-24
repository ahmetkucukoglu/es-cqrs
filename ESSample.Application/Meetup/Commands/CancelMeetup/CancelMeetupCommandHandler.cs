namespace ESSample.Application.Meetup.Commands.CancelMeetup
{
    using Domain.MeetupAggregate;
    using ESSample.Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CancelMeetupCommandHandler : IRequestHandler<CancelMeetupCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public CancelMeetupCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(CancelMeetupCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            aggregate.Cancel(request.OrganizerId);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
