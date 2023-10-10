using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using ActivityApplication.DataAccess;
using ActivityApplication.DataAccess.DbContext;
using ActivityApplication.DataAccess.Users;
using ActivityApplication.Domain.ExceptionsHandling;
using ActivityApplication.Services.Activity;
using ActivityApplication.Services.Activity.Services.Mediator;
using ActivityApplication.Services.User.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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
builder.Services.AddCors();
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
            OnAuthenticationFailed = context => throw new AuthenticationException("Authentication failed."),
            OnTokenValidated = async context =>
            {
                var signInManager = context.HttpContext.RequestServices
                    .GetRequiredService<SignInManager<User>>();

                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                if (claimsIdentity.Claims?.Any() != true)
                    context.Fail("This token has no claims.");

                var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType);
                if (securityStamp == null)
                    context.Fail("This token has no security stamp");

                var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                if (validatedUser == null)
                    context.Fail("Token security stamp is not valid.");
            }
        };
    });


builder.Services.AddAuthorization();


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

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Activity Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddScoped<IActivityService, ActivityService>();

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Mediator Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddMediatR
(cfg => cfg.RegisterServicesFromAssembly(typeof(GetActivityList.Query)
    .Assembly));

//! -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ Membership Services -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
builder.Services.AddScoped<TokenService>();


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
        .WithOrigins("");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();