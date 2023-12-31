using ActivityApplication.DataAccess.Entities.JoinTables;
using ActivityApplication.Services.DTO;

namespace ActivityApplication.Services.Activity.DTO;

public record struct ActivityDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string City { get; set; }
    public string Venue { get; set; }
    public string HostUsername { get; set; }
    public bool IsCanceled { get; set; }
    public List<AttendeeDto> Attendees { get; set; }
}