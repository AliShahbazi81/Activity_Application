using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ActivityApplication.Infrastructure.Security;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        return Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public string? GetUserUsername()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    }

    public string? GetUserRole()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
    }

    public string GetUserFullName()
    {
        return $"""
                {_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.GivenName)}
                            {_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Surname)}
                """;
    }
}