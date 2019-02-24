namespace ESSample.Domain.MeetupAggregate
{
    using System;

    public class OrganizerId : Value<OrganizerId>
    {
        public static readonly OrganizerId Default = new OrganizerId(Guid.Empty);

        public readonly Guid Value;

        internal OrganizerId(Guid value) => Value = value;

        public static OrganizerId Parse(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainValidationException("Organizer is required.");
            }

            return new OrganizerId(value);
        }

        public static implicit operator Guid(OrganizerId self) => self.Value;
        public static implicit operator OrganizerId(Guid value) => Parse(value);
        public static implicit operator string(OrganizerId self) => self.Value.ToString();
    }
}
