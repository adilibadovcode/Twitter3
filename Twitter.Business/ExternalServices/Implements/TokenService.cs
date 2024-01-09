using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Twitter.Business.Dtos.AuthoDtos;
using Twitter.Business.ExternalServices.Interfaces;
using Twitter.Core.Entities;

namespace Twitter.Business.ExternalServices.Implements;
public class TokenService : ITokenService
{
    IConfiguration _config { get; }

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public TokenDto CreateToken(AppUser user)
    {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Name));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        DateTime expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_config.GetSection("Jwt")?["ExpireMin"]));
        JwtSecurityToken jwt = new JwtSecurityToken
            (
            _config.GetSection("Jwt")?["Issuer"],
            _config.GetSection("Jwt")?["Audience"],
            claims,
            DateTime.UtcNow,
            expires,
            cred
            );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwt);
        return new TokenDto
        {
            Token = token,
            Expires = expires,
        };
    }

}
