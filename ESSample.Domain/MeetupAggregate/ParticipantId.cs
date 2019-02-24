namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class ParticipantId : Value<ParticipantId>
    {
        public static readonly ParticipantId Default = new ParticipantId(Guid.Empty);

        public readonly Guid Value;

        internal ParticipantId(Guid value) => Value = value;

        public static ParticipantId Parse(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainValidationException("Participant is required.");
            }

            return new ParticipantId(value);
        }

        public static implicit operator Guid(ParticipantId self) => self.Value;
        public static implicit operator ParticipantId(Guid value) => Parse(value);
        public static implicit operator string(ParticipantId self) => self.Value.ToString();
    }
}
