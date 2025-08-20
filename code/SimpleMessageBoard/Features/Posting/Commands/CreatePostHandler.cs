using SimpleMessageBoard.Repositories;

namespace SimpleMessageBoard.Features.Posting.Commands;

public class CreatePostHandler(IRepository _repository) : IRequestHandler<CreatePostCommand, HandlerResult>
{
    public Task<HandlerResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        HandlerResult result;

        // Can only add to a message board if you are following it.

        var isFollowingProject = _repository.AlreadyFollowingProject(request.UserName, request.Project);
        if (!isFollowingProject)
        {
            result = HandlerResult.NotFollowing;
        }
        else
        {
            var message = new Post(request.PostedAt, request.UserName, request.Message);

            _repository.AddPost(message, request.Project);

            // ALTERNATIVELY - can return the new Post ID, then use a DISCRIMINATED UNION.
            result = HandlerResult.PostAdded;
        }

        return Task.FromResult(result);
    }
}
