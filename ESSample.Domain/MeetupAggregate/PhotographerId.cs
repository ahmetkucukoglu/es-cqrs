namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class PhotographerId : Value<PhotographerId>
    {
        public static readonly PhotographerId Default = new PhotographerId(Guid.Empty);

        public readonly Guid Value;

        internal PhotographerId(Guid value) => Value = value;

        public static PhotographerId Parse(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainValidationException("Photographer is required.");
            }

            return new PhotographerId(value);
        }

        public static implicit operator Guid(PhotographerId self) => self.Value;
        public static implicit operator PhotographerId(Guid value) => Parse(value);
        public static implicit operator string(PhotographerId self) => self.Value.ToString();
    }
}
