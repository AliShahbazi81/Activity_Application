namespace ActivityApplication.Services.Image.DTO;

public record struct ImageUploadDto
{
    public string PublicId { get; set; }
    public string Url { get; set; }
}