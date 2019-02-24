namespace ESSample.Domain.MeetupAggregate
{
    public class MeetupDomainException : DomainException
    {
        public MeetupDomainException(string message) : base(message) { }
    }
}
