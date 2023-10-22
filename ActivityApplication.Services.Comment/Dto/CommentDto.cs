namespace ActivityApplication.Services.Comment.Dto;

public record struct CommentDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Body { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Image { get; set; }
}