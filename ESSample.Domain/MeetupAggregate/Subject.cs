namespace ESSample.Domain.MeetupAggregate
{
    public class Subject : Value<Subject>
    {
        public static readonly Subject Default = new Subject(string.Empty);

        public readonly string Value;

        internal Subject(string value) => Value = value;

        public static Subject Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Subject is required.");
            }

            return new Subject(value);
        }

        public static implicit operator string(Subject self) => self.Value;
        public static implicit operator Subject(string value) => Parse(value);
    }
}
