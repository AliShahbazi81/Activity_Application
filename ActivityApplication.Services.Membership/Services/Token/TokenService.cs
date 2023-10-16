using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActivityApplication.Services.User.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ActivityApplication.Services.User.Services.Token;

public class TokenService
{
    private readonly UserManager<DataAccess.Entities.Users.User> _userManager;
    private readonly SignInManager<DataAccess.Entities.Users.User> _signInManager;
    private readonly IConfiguration _configuration;

    public TokenService(UserManager<DataAccess.Entities.Users.User> userManager, SignInManager<DataAccess.Entities.Users.User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    public async Task<string> GenerateToken(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new UserNotFoundException(userId.ToString());

        // 1. Create a list of claims
        //! These credentials will be sent to the client using payload
        var claimsList = new List<Claim>
        {
            new(ClaimTypes.GivenName, user.Name),
            new(ClaimTypes.Surname, user.Surname),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        claimsList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var secretKey = Encoding.UTF8.GetBytes(_configuration["JWTSettings:TokenKey"]!); // longer that 16 character
        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionKey = Encoding.UTF8.GetBytes(_configuration["JWTSettings:EncryptKey"]!); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey),
            SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var claimsPrincipal = await _signInManager.ClaimsFactory.CreateAsync(user);

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWTSettings:Issuer"],
            Audience = _configuration["JWTSettings:Audience"],
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claimsPrincipal.Claims),
            Expires = DateTime.UtcNow.AddDays(7)
        });

        var accessToken = tokenHandler.WriteToken(securityToken);
        // return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return accessToken;
    }
}