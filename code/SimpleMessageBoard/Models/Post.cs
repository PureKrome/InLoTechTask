namespace SimpleMessageBoard.Repositories;

    public record Post(
        DateTime PostedAt,
        string UserName,
        string Message);
