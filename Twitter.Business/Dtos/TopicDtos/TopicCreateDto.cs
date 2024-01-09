using FluentValidation;

namespace Twitter.Business.Dtos.TopicDtos;
public class TopicCreateDto
{
    public string Name { get; set; }
}
public class TopicCreateDtoValidator : AbstractValidator<TopicCreateDto>
{
    public TopicCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32).MinimumLength(2);
    }
}
