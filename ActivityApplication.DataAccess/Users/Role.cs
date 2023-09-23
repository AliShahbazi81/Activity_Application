using Microsoft.AspNetCore.Identity;

namespace ActivityApplication.DataAccess.Users;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
    }
    
}