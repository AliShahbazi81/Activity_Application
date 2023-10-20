namespace ActivityApplication.Services.Activity.DTO;

public record struct AttendeeDto
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
}