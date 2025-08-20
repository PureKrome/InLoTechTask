using SimpleMessageBoard.Models;

namespace SimpleMessageBoard.Repositories;

public interface IRepository
{
    void AddPost(Post post, string project);
    List<Post> GetPosts(string project);

    bool AlreadyFollowingProject(string userName, string project);

    bool DoesProjectExist(string project);

    void FollowProject(string userName, string project);
    void UnfollowProject(string userName, string project);
}
