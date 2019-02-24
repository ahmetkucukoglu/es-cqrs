namespace ESSample.Domain.MeetupAggregate.Exceptions
{
    public class MeetupNotFoundException : DomainException
    {
        public MeetupNotFoundException() : base("Meetup not found.") { }
    }
}
