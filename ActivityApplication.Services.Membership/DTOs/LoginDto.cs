namespace ActivityApplication.Services.User.DTOs;

public record struct LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}