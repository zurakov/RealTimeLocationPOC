namespace RealTimeLocationPOC.Api.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string FullName { get; set; }
        public bool IsOnline { get; set; }
        public DateTimeOffset? LastSeenAt { get; set; }
    }
}
