using OneOf;

namespace SimpleMessageBoard.Features.Reading.Queries;

public record ReadProjectQuery(string project) : IRequest<OneOf<HandlerResult, List<Post>>>;
