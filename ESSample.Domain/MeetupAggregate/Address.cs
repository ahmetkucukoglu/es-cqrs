namespace ESSample.Domain.MeetupAggregate
{
    public class Address : Value<Address>
    {
        public static readonly Address Default = new Address(string.Empty);

        public readonly string Value;

        internal Address(string value) => Value = value;

        public static Address Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Address is required.");
            }

            return new Address(value);
        }

        public static implicit operator string(Address self) => self.Value;
        public static implicit operator Address(string value) => Parse(value);
    }
}
