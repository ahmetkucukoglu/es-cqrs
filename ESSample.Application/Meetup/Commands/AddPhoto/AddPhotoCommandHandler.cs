namespace ESSample.Application.Meetup.Commands.AddPhoto
{
    using Domain.MeetupAggregate;
    using ESSample.Infrastructure;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, Unit>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public AddPhotoCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<Unit> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _aggregateRepository.LoadAsync<Meetup>(request.MeetupId.ToString(), cancellationToken);
            aggregate.AddPhoto(request.PhotographerId, request.Photo);

            await _aggregateRepository.SaveAsync(aggregate);

            return Unit.Value;
        }
    }
}
