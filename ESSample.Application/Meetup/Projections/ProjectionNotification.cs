namespace ESSample.Application.Meetup.Projections
{
    using Domain;
    using MediatR;

    public class ProjectionNotification : INotification
    {
        public DomainEvent DomainEvent { get; set; }
    }
}
