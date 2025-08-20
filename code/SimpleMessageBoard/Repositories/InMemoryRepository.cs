namespace SimpleMessageBoard.Repositories;

public class InMemoryRepository : IRepository
{
    private readonly Dictionary<string, ProjectData> _projects = [];
    
    public void AddPost(Post post, string project)
    {
        var existingProject = EnsureProjectExists(project);

        existingProject.Posts.Add(post);
    }

    public List<Post> GetPosts(string project)
    {
        var existingProject = EnsureProjectExists(project);

        // Ordered by most recent first.
        return existingProject.Posts
            .OrderByDescending(p => p.PostedAt)
            .ToList();
    }

    public bool AlreadyFollowingProject(string userName, string project)
    {
        var existingProject = EnsureProjectExists(project);

        return existingProject.Followers.Contains(userName);
    }

    public void FollowProject(string userName, string project)
    {
        var existingProject = EnsureProjectExists(project);

        if (!existingProject.Followers.Contains(userName))
        {
            existingProject.Followers.Add(userName);
        }
        else
        {
            throw new Exception($"User {userName} is already following this project.");
        }
    }

    public void UnfollowProject(string userName, string project)
    {
        // Not a requirement, but a suggestion for future implementation.
        throw new NotImplementedException();
    }

    private ProjectData EnsureProjectExists(string project)
    {
        if (!_projects.ContainsKey(project))
        {
            _projects[project] = new ProjectData([], []);
        }

        // Either the newly created project or an existing one is returned.
        return _projects[project];
    }
}
