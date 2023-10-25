using ActivityApplication.DataAccess.Entities.Users;

namespace ActivityApplication.Services.DTO;

public record ProfileDto
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Image { get; set; }

    // To understand if the returned user is following that particular user
    public bool Following { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public List<ImageUploadDto>? Photos { get; set; }
}