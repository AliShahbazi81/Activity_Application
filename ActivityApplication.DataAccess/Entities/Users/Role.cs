using Microsoft.AspNetCore.Identity;

namespace ActivityApplication.DataAccess.Entities.Users;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
    }
    
}