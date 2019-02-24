namespace ESSample.Domain.MeetupAggregate.Exceptions
{
    public class MeetupNotCompletedException : DomainException
    {
        public MeetupNotCompletedException() : base("Meetup is not completed.") { }
    }
}
