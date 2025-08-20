namespace SimpleMessageBoard.Features.Posting.Commands;

public record CreatePostCommand(
    DateTime PostedAt,
    string UserName,
    string Project,
    string Message) : IRequest<HandlerResult>; 
