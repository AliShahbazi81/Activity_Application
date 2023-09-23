using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Users;
using Microsoft.AspNetCore.Identity;

namespace ActivityApplication.DataAccess;

public static class DbInitializer
{
    public static async Task Initializer(ApplicationDbContext dbContext, UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        // Seeding User
        if (dbContext.Users.Any())
            return;

        var admin = new Role
        {
            Name = "Admin"
        };
        await roleManager.CreateAsync(admin);

        var member = new Role
        {
            Name = "Member"
        };
        await roleManager.CreateAsync(member);


        var user = new User
        {
            UserName = "admin",
            Name = "User",
            Surname = "UserSurname",
            Email = "alishahbazi799@gmail.com",
            LockoutEnabled = false
        };

        await userManager.CreateAsync(user, "@GG001323132300gg");
        await userManager.AddToRoleAsync(user, "Admin");

        var memberUser = new User
        {
            UserName = "memberUser",
            Name = "Member",
            Surname = "MemberSurname",
            Email = "alishahbazi899@gmail.com",
            LockoutEnabled = false
        };

        await userManager.CreateAsync(memberUser, "@GG001323132300gg");
        await userManager.AddToRoleAsync(memberUser, "Member");

        var testUser = new User
        {
            UserName = "testUser",
            Name = "Test",
            Surname = "TestSurname",
            Email = "alishahbazi999@gmail.com",
            LockoutEnabled = false
        };

        await userManager.CreateAsync(testUser, "@GG001323132300gg");
        await userManager.AddToRoleAsync(testUser, "Member");
    }
}