namespace ActivityApplication.Services.DTO;

public record ImageUploadDto
{
    public string? PublicId { get; set; }
    public string? Url { get; set; }
}