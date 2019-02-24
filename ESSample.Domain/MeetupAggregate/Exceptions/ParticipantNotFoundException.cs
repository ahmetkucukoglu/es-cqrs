namespace ESSample.Domain.MeetupAggregate.Exceptions
{
    public class ParticipantNotFoundException : DomainException
    {
        public ParticipantNotFoundException() : base("Pariticipant not found.") { }
    }
}
