namespace ESSample.Domain.MeetupAggregate
{
    public class Longitude : Value<Longitude>
    {
        public static readonly Longitude Default = new Longitude(string.Empty);

        public readonly string Value;

        internal Longitude(string value) => Value = value;

        public static Longitude Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Longitude is required.");
            }

            return new Longitude(value);
        }

        public static implicit operator string(Longitude self) => self.Value;
        public static implicit operator Longitude(string value) => Parse(value);
    }
}
