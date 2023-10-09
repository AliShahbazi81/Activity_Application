namespace ActivityApplication.Services.User.DTOs;

public struct RegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
    public string Username { get; set; }
}