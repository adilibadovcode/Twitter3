using FluentValidation;
using Twitter.Business.Dtos.TopicDtos;

namespace Twitter.Business.Dtos.AuthoDtos;
public class RegisterDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
}
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(32);
        RuleFor(x => x.Surname).NotEmpty().MinimumLength(2).MaximumLength(32);
        RuleFor(x => x.Username).NotEmpty().MinimumLength(2).MaximumLength(64);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
                     .EmailAddress().WithMessage("A valid email is required");
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Birth Date address is required")
            .LessThan(x => DateTime.Now).WithMessage("A valid Birth Date is required"); ;
    }
}
