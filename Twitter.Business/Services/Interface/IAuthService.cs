using Twitter.Business.Dtos.AuthoDtos;

namespace Twitter.Business.Services.Interface;

public interface IAuthService
{
    Task<TokenDto> Login(LoginDto dto);
}
