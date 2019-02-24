namespace ESSample.Domain.MeetupAggregate
{
    using System;
        
    public class When : Value<When>
    {
        public static readonly When Default = new When(DateTime.Now);

        public readonly DateTime Value;

        internal When(DateTime value) => Value = value;

        public static When Parse(DateTime value)
        {
            if (value == null)
            {
                throw new DomainValidationException("When is required.");
            }

            if (value <= DateTime.Today)
            {
                throw new DomainValidationException("When must greater than today.");
            }

            return new When(value);
        }

        public static implicit operator DateTime(When self) => self.Value;
        public static implicit operator When(DateTime value) => Parse(value);
    }
}
