using ActivityApplication.DataAccess.Activities;
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
        
        if (dbContext.Activities.Any()) return;
            
            var activities = new List<Activity>
            {
                new()
                {
                    Title = "Past Activity 1",
                    Date = DateTime.UtcNow.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new()
                {
                    Title = "Past Activity 2",
                    Date = DateTime.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                },
                new()
                {
                    Title = "Future Activity 1",
                    Date = DateTime.UtcNow.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                },
                new()
                {
                    Title = "Future Activity 2",
                    Date = DateTime.UtcNow.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                },
                new()
                {
                    Title = "Future Activity 3",
                    Date = DateTime.UtcNow.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Another pub",
                },
                new()
                {
                    Title = "Future Activity 4",
                    Date = DateTime.UtcNow.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Yet another pub",
                },
                new()
                {
                    Title = "Future Activity 5",
                    Date = DateTime.UtcNow.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Just another pub",
                },
                new()
                {
                    Title = "Future Activity 6",
                    Date = DateTime.UtcNow.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "Roundhouse Camden",
                },
                new()
                {
                    Title = "Future Activity 7",
                    Date = DateTime.UtcNow.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "travel",
                    City = "London",
                    Venue = "Somewhere on the Thames",
                },
                new()
                {
                    Title = "Future Activity 8",
                    Date = DateTime.UtcNow.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "film",
                    City = "London",
                    Venue = "Cinema",
                }
            };

            await dbContext.Activities.AddRangeAsync(activities);
            await dbContext.SaveChangesAsync();
    }
}