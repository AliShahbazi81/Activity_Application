namespace ActivityApplication.Infrastructure.Security;

public interface IUserAccessor
{
    Guid GetUserId();
    string? GetUserUsername();
    string? GetUserRole();
    string GetUserFullName();
}