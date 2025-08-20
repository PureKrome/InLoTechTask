using Spectre.Console;

namespace SimpleMessageBoard.Services;

// NOTE: PRIMARY A.I. GENERATED.
// HERE BE DRAGONS! 🐉🐉

public static class SpectreDisplayService
{
    public static void DisplayPosts(List<Post> posts)
    {
        if (!posts.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No posts found.[/]");
            return;
        }

        var table = new Table()
            .BorderColor(Color.Grey)
            .AddColumn("[cyan]User[/]")
            .AddColumn("[cyan]Message[/]")
            .AddColumn("[cyan]Posted[/]");

        foreach (var post in posts)
        {
            var timeAgo = GetTimeAgo(post.PostedAt);
            table.AddRow(
                $"[bold]{post.UserName}[/]",
                post.Message,
                $"[grey]{timeAgo}[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayWallPosts(List<Post> posts)
    {
        if (!posts.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No posts found.[/]");
            return;
        }

        var table = new Table()
            .BorderColor(Color.Grey)
            .AddColumn("[cyan]Project[/]")
            .AddColumn("[cyan]User[/]")
            .AddColumn("[cyan]Message[/]")
            .AddColumn("[cyan]Posted[/]");

        foreach (var post in posts)
        {
            var timeAgo = GetTimeAgo(post.PostedAt);
            table.AddRow(
                //$"[bold blue]{post.Project}[/]",
                $"[bold]{post.UserName}[/]",
                post.Message,
                $"[grey]{timeAgo}[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayWelcomeMessage()
    {
        var rule = new Rule("[bold cyan]Simple Message Board[/]")
        {
            Style = Style.Parse("cyan")
        };
        AnsiConsole.Write(rule);

        var panel = new Panel(new Markup(
            "[bold]Commands:[/]\n" +
            "[cyan]  <user name> -> @<project name> <message>[/] - Post a message\n" +
            "[cyan]  <project name>[/] - Read project timeline\n" +
            "[cyan]  <user name> follows <project name>[/] - Follow a project\n" +
            "[cyan]  <user name> wall[/] - View user's wall\n" +
            "[grey]  Type 'exit' to quit[/]"))
        {
            Header = new PanelHeader(" [bold]Getting Started[/] "),
            Border = BoxBorder.Rounded,
            BorderStyle = Style.Parse("cyan")
        };
        
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    private static string GetTimeAgo(DateTime postTime)
    {
        var timeSpan = DateTime.Now - postTime;

        if (timeSpan.TotalMinutes < 1)
            return "just now";

        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes >= 2 ? "s" : "")} ago";

        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours >= 2 ? "s" : "")} ago";

        return $"{(int)timeSpan.TotalDays} day{(timeSpan.TotalDays >= 2 ? "s" : "")} ago";
    }
}