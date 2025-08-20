namespace SimpleMessageBoard.Repositories;

public class InMemoryRepository : IRepository
{
    // Fake Table: Posts
    //   - PK: project name
    //   - List of people following the project.
    private readonly Dictionary<string, List<string>> _followers = [];

    // Fake Table: Posts
    //   - PK: project name
    //   - List of posts.
    private readonly Dictionary<string, List<Post>> _posts = [];
    
    public void AddPost(Post post)
    {
        if (!_posts.TryGetValue(post.Project, out var value))
        {
            // We don't have any projects
            value = [];
            _posts[post.Project] = value;
        }

        value.Add(post);
    }

    public void FollowProject(string userName)
    {
        if (!_followers.TryGetValue(userName, out var followers))
        {
            // We don't have any followers for this user.
            followers = [];
            _followers[userName] = followers;
        }
        if (!followers.Contains(userName))
        {
            followers.Add(userName);
        }
        else
        {
            throw new Exception($"User {userName} is already following this project.");
        }
    }

    public List<Post> GetPosts(string project)
    {
        if (_posts.TryGetValue(project, out var posts))
        {
            return posts;
        }
        else
        {
            // No posts for this project.
            return [];
        }
    }

    public void UnfollowProject(string userName)
    {
        throw new NotImplementedException();
    }
}
