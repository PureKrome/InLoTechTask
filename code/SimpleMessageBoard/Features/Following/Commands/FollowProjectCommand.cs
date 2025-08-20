namespace SimpleMessageBoard.Features.Following.Commands;

public record FollowProjectCommand(
    string UserName,
    string ProjectName) : IRequest<bool>;
