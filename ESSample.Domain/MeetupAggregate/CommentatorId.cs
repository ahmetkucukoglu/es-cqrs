namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class CommentatorId : Value<CommentatorId>
    {
        public static readonly CommentatorId Default = new CommentatorId(Guid.Empty);

        public readonly Guid Value;

        internal CommentatorId(Guid value) => Value = value;

        public static CommentatorId Parse(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainValidationException("Commentator is required.");
            }

            return new CommentatorId(value);
        }

        public static implicit operator Guid(CommentatorId self) => self.Value;
        public static implicit operator CommentatorId(Guid value) => Parse(value);
        public static implicit operator string(CommentatorId self) => self.Value.ToString();
    }
}
