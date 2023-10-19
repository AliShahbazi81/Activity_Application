using ActivityApplication.Domain.Results;
using ActivityApplication.Services.DTO;

namespace ActivityApplication.Services.User.Services;

public interface IUserService
{
    Task<Result<ProfileDto>> GetUserProfileByUsernameAsync(string userName);
}