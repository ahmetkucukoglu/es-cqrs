namespace ESSample.Application.Meetup.Commands.CreateMeetup
{
    using Domain.MeetupAggregate;
    using Domain.MeetupAggregate.Policies;
    using Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateMeetupCommandHandler : IRequestHandler<CreateMeetupCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IMeetupPolicy _meetupPolicy;

        public CreateMeetupCommandHandler(IAggregateRepository aggregateRepository, IMeetupPolicy meetupPolicy)
        {
            _aggregateRepository = aggregateRepository;
            _meetupPolicy = meetupPolicy;
        }

        public async Task<Unit> Handle(CreateMeetupCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            await aggregate.Register(request.MeetupId, request.OrganizerId, request.Subject, request.When, request.Description, request.Address, _meetupPolicy);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
