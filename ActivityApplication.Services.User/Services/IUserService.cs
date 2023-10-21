using ActivityApplication.Domain.Results;
using ActivityApplication.Services.DTO;
using ActivityApplication.Services.User.Dto;

namespace ActivityApplication.Services.User.Services;

public interface IUserService
{
    Task<Result<ProfileDto>> GetUserProfileByUsernameAsync(string userName);
    Task<Result<string>> EditUserProfileById(Guid userId, UserEditDto userEditDto);
}