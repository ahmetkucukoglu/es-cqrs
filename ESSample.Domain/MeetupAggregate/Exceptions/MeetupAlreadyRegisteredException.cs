namespace ESSample.Domain.MeetupAggregate.Exceptions
{
    public class MeetupAlreadyRegisteredException : DomainException
    {
        public MeetupAlreadyRegisteredException() : base("Meetup already registered.") { }
    }
}
