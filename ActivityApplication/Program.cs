using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using ActivityApplication.DataAccess;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Entities.Users;
using ActivityApplication.DataAccess.Seed;
using ActivityApplication.Domain.ExceptionsHandling;
using ActivityApplication.Infrastructure.Security;
using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.Services.Mediator;
using ActivityApplication.Services.User.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//! This means that all of the controllers do need Authorization
builder.Services.AddControllers();
//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Swagger Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt auth header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Database Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddDbContextFactory<ApplicationDbContext>(opt => { opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); });

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Authentication Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddCors(options => options.AddPolicy("cors", policy => policy
    .SetIsOriginAllowed(origin =>
        new Uri(origin).Host.Contains("localhost")
    )
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()));

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Identity Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
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
    // It allows us to query our User entity
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        // It shows how we want to validate the token
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "localhost",
            ValidAudience = "localhost",
            // This will validate the signature of the token and if the SigningKey is not equal with the one in the token, it will throw an exception
            // ValidateIssuerSigningKey = true,
            //! builder.Configuration must be equal with the one that we created in the TokenService
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"]!)),
            TokenDecryptionKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:EncryptKey"]!))
        };
        opt.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Exception, "Authentication failed.");
                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                var signInManager = context.HttpContext.RequestServices
                    .GetRequiredService<SignInManager<User>>();
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                if (claimsIdentity.Claims?.Any() != true)
                {
                    logger.LogWarning("Token has no claims.");
                    context.Fail("This token has no claims.");
                }

                var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType);
                if (securityStamp == null)
                {
                    logger.LogWarning("Token has no security stamp.");
                    context.Fail("This token has no security stamp");
                }

                var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                if (validatedUser == null)
                {
                    logger.LogWarning("Token security stamp is not valid.");
                    context.Fail("Token security stamp is not valid.");
                }
            }
        };
    });


builder.Services.AddAuthorization();

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Activity Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddScoped<IActivityService, ActivityService>();

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Mediator Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddMediatR
(cfg => cfg.RegisterServicesFromAssembly(typeof(GetActivityList.Query)
    .Assembly));

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Membership Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddScoped<TokenService>();

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ User Accessor Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

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

app.UseStaticFiles();

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

app.UseCors("cors");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Add our customized Exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();