namespace ActivityApplication.Services.User.Dto;

public record struct UserEditDto
{
    public string DisplayName { get; set; }
    public string Bio { get; set; }
}