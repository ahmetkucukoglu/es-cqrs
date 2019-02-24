namespace ESSample.Domain.MeetupAggregate
{
    public class Photo : Value<Photo>
    {
        public static readonly Photo Default = new Photo(string.Empty);

        public readonly string Value;

        internal Photo(string value) => Value = value;

        public static Photo Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Photo is required.");
            }

            return new Photo(value);
        }

        public static implicit operator string(Photo self) => self.Value;
        public static implicit operator Photo(string value) => Parse(value);
    }
}
