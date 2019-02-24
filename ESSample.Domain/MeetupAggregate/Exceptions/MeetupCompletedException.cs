namespace ESSample.Domain.MeetupAggregate.Exceptions
{
    public class MeetupCompletedException : DomainException
    {
        public MeetupCompletedException() : base("Meetup is completed.") { }
    }
}
