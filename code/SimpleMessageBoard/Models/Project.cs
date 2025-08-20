namespace SimpleMessageBoard.Models;

public class Project : IProject
{
    private readonly List<string> _users = [];
    private readonly List<Post> _posts = [];

    public IReadOnlyList<string> Users => _users.AsReadOnly();

    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

    public void Follow(string userName)
    {
        if (!_users.Contains(userName))
        {
            _users.Add(userName);
        }
        else
        {
            throw new Exception($"User {userName} is already following this project.");
        }
    }

    public void Unfollow(string userName)
    {
        if (!_users.Remove(userName))
        {
            throw new Exception($"User {userName} is not following this project.");
        }
    }

    public void AddPost(Post post)
    {
        ArgumentNullException.ThrowIfNull(post);

        _posts.Add(post);
    }
}
