namespace SimpleMessageBoard.Models;

    public record Post(
        DateTime PostedAt,
        string UserName,
        string Project,
        string Message);
