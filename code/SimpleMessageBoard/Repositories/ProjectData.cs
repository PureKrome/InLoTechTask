namespace SimpleMessageBoard.Repositories;

internal record ProjectData(
    List<Post> Posts,
    List<string> Followers);
