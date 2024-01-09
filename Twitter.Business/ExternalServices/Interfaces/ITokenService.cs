using Twitter.Business.Dtos.AuthoDtos;
using Twitter.Core.Entities;

namespace Twitter.Business.ExternalServices.Interfaces;
public interface ITokenService
{
    TokenDto CreateToken(AppUser user);
}
