namespace RealTimeLocationPOC.Web.Models
{
    public class LocationUpdate
    {
        public Guid EmployeeId { get; set; }
        public string FullName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float? Heading { get; set; }
        public bool IsOnline { get; set; }
        public DateTimeOffset LastSeenAt { get; set; }
    }
}
