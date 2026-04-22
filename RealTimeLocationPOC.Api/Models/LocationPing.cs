namespace RealTimeLocationPOC.Api.Models
{
    public class LocationPing
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float? Heading { get; set; }
        public DateTimeOffset RecordedAt { get; set; }
    }
}
