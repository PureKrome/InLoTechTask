using SimpleMessageBoard.Models;

namespace SimpleMessageBoard.Repositories;

public interface IRepository
{
    void AddPost(Post post);
    List<Post> GetPosts(string project);

    void FollowProject(string userName);
    void UnfollowProject(string userName);
}
