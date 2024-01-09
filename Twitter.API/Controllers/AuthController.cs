using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Twitter.Business.Dtos.AuthoDtos;
using Twitter.Business.ExternalServices.Interfaces;
using Twitter.Business.Services.Interface;
using Twitter.Core.Entities;

namespace Twitter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        IAuthService _authService { get; }

        public AuthController(IEmailService emailService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto vm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    UserName = vm.Username,
                    Surname = vm.Surname,
                    BirthDate = vm.BirthDate,
                };
                var result = await _userManager.CreateAsync(user, vm.Password);

            }

            return Ok();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            //AppUser user;
            //user = await _userManager.FindByNameAsync(dto.Username);
            //await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
            //_emailService.Send(user.Email, "hello", "hello");
            return Ok(await _authService.Login(dto));
        }
    }
}
