namespace ESSample.Application.Meetup.Commands.CompleteMeetup
{
    using Domain.MeetupAggregate;
    using ESSample.Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CompleteMeetupCommandHandler : IRequestHandler<CompleteMeetupCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public CompleteMeetupCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(CompleteMeetupCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            aggregate.Complete(request.OrganizerId);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
