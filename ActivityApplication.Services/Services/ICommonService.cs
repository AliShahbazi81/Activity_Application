namespace ActivityApplication.Services.Services;

public interface ICommonService
{
    Task<string?> GetUserMainPhotoAsync(Guid userId);
}