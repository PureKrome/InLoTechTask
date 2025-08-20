
using SimpleMessageBoard.Repositories;

namespace SimpleMessageBoard.Features.Posting.Commands;

public class CreatePostHandler(IRepository _repository) : IRequestHandler<CreatePostCommand>
{
    public Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        // Can only add to a message board if you are following it.

        var message = new Post(request.PostedAt, request.UserName, request.Project, request.Message);

        _repository.AddPost(message);

        return Task.CompletedTask;
    }
}
