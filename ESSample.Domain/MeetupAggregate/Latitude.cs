namespace ESSample.Domain.MeetupAggregate
{
    public class Latitude : Value<Latitude>
    {
        public static readonly Latitude Default = new Latitude(string.Empty);

        public readonly string Value;

        internal Latitude(string value) => Value = value;

        public static Latitude Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Latitude is required.");
            }

            return new Latitude(value);
        }

        public static implicit operator string(Latitude self) => self.Value;
        public static implicit operator Latitude(string value) => Parse(value);
    }
}
