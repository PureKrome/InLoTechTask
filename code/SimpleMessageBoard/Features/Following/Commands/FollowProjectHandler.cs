using SimpleMessageBoard.Repositories;

namespace SimpleMessageBoard.Features.Following.Commands;

public class FollowProjectHandler(IRepository _repository) : IRequestHandler<FollowProjectCommand, bool>
{
    public Task<bool> Handle(FollowProjectCommand request, CancellationToken cancellationToken)
    {
        // Can only follow if we aren't already following.
        var didFollow = false;

        var isFollowingProject = _repository.AlreadyFollowingProject(request.UserName, request.ProjectName);
        if (!isFollowingProject)
        {
            _repository.FollowProject(request.UserName, request.ProjectName);
            didFollow = true;
        }

        return Task.FromResult(didFollow);
    }
}
