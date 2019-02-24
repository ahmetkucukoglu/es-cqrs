namespace ESSample.Application.Meetup.Commands.UpdateMeetup
{
    using Domain.MeetupAggregate;
    using Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateMeetupCommandHandler : IRequestHandler<UpdateMeetupCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public UpdateMeetupCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(UpdateMeetupCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            aggregate.ChangeSubject(request.OrganizerId, request.Subject);
            aggregate.ChangeWhen(request.OrganizerId, request.When);
            aggregate.ChangeDescription(request.OrganizerId, request.Description);
            aggregate.ChangeAddress(request.OrganizerId, request.Address);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
