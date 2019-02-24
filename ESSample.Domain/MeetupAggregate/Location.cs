namespace ESSample.Domain.MeetupAggregate
{
    public class Location
    {
        public Address Address { get; set; }
        public Latitude Latitude { get; set; }
        public Longitude Longitude { get; set; }
    }
}
