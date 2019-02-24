namespace ESSample.Domain
{
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message) : base(message) { }
    }
}
