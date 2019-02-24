namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class MeetupId : Value<MeetupId>
    {
        public static readonly MeetupId Default = new MeetupId(Guid.Empty);

        public readonly Guid Value;

        internal MeetupId(Guid value) => Value = value;

        public static MeetupId Parse(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainValidationException("Meetup is required.");
            }

            return new MeetupId(value);
        }

        public static implicit operator Guid(MeetupId self) => self.Value;
        public static implicit operator MeetupId(Guid value) => Parse(value);
        public static implicit operator string(MeetupId self) => self.Value.ToString();
    }
}
