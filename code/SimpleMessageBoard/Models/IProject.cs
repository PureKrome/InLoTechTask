namespace SimpleMessageBoard.Models;

public interface IProject
{
    void Follow(string userName);
    void Unfollow(string userName);
}
