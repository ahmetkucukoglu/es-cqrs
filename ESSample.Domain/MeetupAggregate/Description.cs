namespace ESSample.Domain.MeetupAggregate
{
    public class Description : Value<Description>
    {
        public static readonly Description Default = new Description(string.Empty);

        public readonly string Value;

        internal Description(string value) => Value = value;

        public static Description Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Description is required.");
            }

            return new Description(value);
        }

        public static implicit operator string(Description self) => self.Value;
        public static implicit operator Description(string value) => Parse(value);
    }
}
