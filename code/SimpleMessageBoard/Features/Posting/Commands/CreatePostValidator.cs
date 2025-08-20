using FluentValidation;


namespace SimpleMessageBoard.Features.Posting.Commands;

public class CreatePostValidator : AbstractValidator<Post>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Post UserName is required.")
            .MinimumLength(3)
            .WithMessage("Post UserName must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("Post UserName cannot exceed 100 characters.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Post Message is required.")
            .MinimumLength(10)
            .WithMessage("Post Message must be at least 10 characters long.")
            .MaximumLength(5000)
            .WithMessage("Post Message cannot exceed 5000 characters.");

        RuleFor(x => x.PostedAt)
            .NotEqual(DateTime.MinValue)
            .WithMessage("Post PostedAt cannot be that weird default Date/Time value/");
    }
}
