using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Twitter.Business.Dtos.AuthoDtos;
using Twitter.Business.Exceptions.Auth;
using Twitter.Business.ExternalServices.Interfaces;
using Twitter.Business.Services.Interface;
using Twitter.Core.Entities;

namespace Twitter.Business.Services.Implements;
public class AuthService : IAuthService
{
    UserManager<AppUser> _userManager { get; }
    ITokenService _tokenService { get; }
    RoleManager<IdentityRole> _roleManager { get; }
    public AuthService(UserManager<AppUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<TokenDto> Login(LoginDto dto)
    {
        AppUser? user = null;
        if (dto.UsernameOrEmail.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(dto.UsernameOrEmail);
        }
        else
        {
            user = await _userManager.FindByNameAsync(dto.UsernameOrEmail);
        }
        if (user == null) throw new UsernameOrPasswordNotFoundException();
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Name));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a47cfa0b-68e6-4e2a-9982-ff0265fc549c"));
        SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwt = new JwtSecurityToken
            (
            "https://localhost:7235/",
            "https://localhost:7235/api",
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24),
            cred
            );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwt);
        return _tokenService.CreateToken(user);
    }
}
