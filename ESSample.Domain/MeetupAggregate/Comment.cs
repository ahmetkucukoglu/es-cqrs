namespace ESSample.Domain.MeetupAggregate
{
    public class Comment : Value<Comment>
    {
        public static readonly Comment Default = new Comment(string.Empty);

        public readonly string Value;

        internal Comment(string value) => Value = value;

        public static Comment Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException("Comment is required.");
            }

            return new Comment(value);
        }

        public static implicit operator string(Comment self) => self.Value;
        public static implicit operator Comment(string value) => Parse(value);
    }
}
