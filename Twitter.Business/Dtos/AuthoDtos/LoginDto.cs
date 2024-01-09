using FluentValidation;

namespace Twitter.Business.Dtos.AuthoDtos;
public class LoginDto
{
    public string UsernameOrEmail { get; set; }

    public string Password { get; set; }
}

public class LoginDtoVlaidator : AbstractValidator<LoginDto>
{
    public LoginDtoVlaidator()
    {
        RuleFor(a => a.UsernameOrEmail)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(64);
        RuleFor(a => a.Password)
          .NotEmpty()
          .NotNull()
          .MinimumLength(6)
          .MaximumLength(64);
    }
}