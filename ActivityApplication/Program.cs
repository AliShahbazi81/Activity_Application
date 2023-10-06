using ActivityApplication.DataAccess;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Users;
using ActivityApplication.Domain.ExceptionsHandling;
using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.Services.Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextFactory<ApplicationDbContext>(opt => { opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); });

builder.Services.AddIdentity<User, Role>(opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 4;
        opt.User.RequireUniqueEmail = true;
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Activity Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddScoped<IActivityService, ActivityService>();

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Mediator Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddMediatR
(cfg => cfg.RegisterServicesFromAssembly(typeof(GetActivityList.Query)
    .Assembly));


var app = builder.Build();

//! Seeding Database
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

try
{
    context.Database.Migrate();
    await DbInitializer.Initializer(context, userManager, roleManager);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

// Add our customized Exception middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(opt =>
{
    opt.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(
            "https://localhost:3000",
            "http://localhost:5162",
            "https://localhost:7290");
});


app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();