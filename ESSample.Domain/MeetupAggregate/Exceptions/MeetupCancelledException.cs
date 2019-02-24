namespace ESSample.Domain.MeetupAggregate.Exceptions
{
    public class MeetupCancelledException : DomainException
    {
        public MeetupCancelledException() : base("Meetup is cancelled.") { }
    }
}
