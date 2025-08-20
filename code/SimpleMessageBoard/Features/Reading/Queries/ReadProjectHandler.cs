using OneOf;
using SimpleMessageBoard.Repositories;

namespace SimpleMessageBoard.Features.Reading.Queries;

public class ReadProjectHandler(IRepository repository) : IRequestHandler<ReadProjectQuery, OneOf<HandlerResult, List<Post>>>
{
    public Task<OneOf<HandlerResult, List<Post>>> Handle(ReadProjectQuery request, CancellationToken cancellationToken)
    {
        var doesProjectExist = repository.DoesProjectExist(request.project);
        if (!doesProjectExist)
        {
            return Task.FromResult<OneOf<HandlerResult, List<Post>>>(HandlerResult.ProjectDoesntExist);
        }

        var posts = repository.GetPosts(request.project);

        return Task.FromResult<OneOf<HandlerResult, List<Post>>>(posts);
    }
}
