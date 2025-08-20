namespace SimpleMessageBoard.Repositories;

public interface IRepository
{
    void AddPost(Post post);
    List<Post> GetPosts(string project);

}
