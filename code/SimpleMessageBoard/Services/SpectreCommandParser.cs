using SimpleMessageBoard.Features.Following.Commands;
using SimpleMessageBoard.Features.Posting.Commands;
using SimpleMessageBoard.Features.Reading.Queries;
using Spectre.Console;

namespace SimpleMessageBoard.Services;

// NOTE: PRIMARY A.I. GENERATED.
// HERE BE DRAGONS! 🐉🐉

public interface ICommandParser
{
    IBaseRequest ParseCommand(string input); // Changed from IRequest to IBaseRequest
}

public class SpectreCommandParser : ICommandParser
{
    public IBaseRequest ParseCommand(string input) // Changed return type
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be empty");
        }

        input = input.Trim();

        try
        {
            return ParseNaturalLanguageCommand(input);
        }
        catch
        {
            // Use Spectre.Console for nice error formatting 🖼️
            AnsiConsole.MarkupLine($"[red]Unknown command format:[/] {input}");
            AnsiConsole.MarkupLine("[yellow]Available commands:[/]");
            AnsiConsole.MarkupLine("  [cyan]<user name> -> @<project name> <message>[/] - Post a message");
            AnsiConsole.MarkupLine("  [cyan]<project name>[/] - Read project timeline");
            AnsiConsole.MarkupLine("  [cyan]<user name> follows <project name>[/] - Follow a project");
            AnsiConsole.MarkupLine("  [cyan]<user name> wall[/] - View user's wall");
            
            throw new ArgumentException($"Unknown command format: {input}");
        }
    }

    private static IBaseRequest ParseNaturalLanguageCommand(string input) // Changed return type
    {
        // Try posting pattern: <user name> -> @<project name> <message>
        if (TryParsePostingCommand(input, out var postCommand))
        {
            return postCommand!;
        }

        // Try following pattern: <user name> follows <project name>
        if (TryParseFollowingCommand(input, out var followCommand))
        {
            return followCommand!;
        }

        //// Try wall pattern: <user name> wall
        //if (TryParseWallCommand(input, out var wallCommand))
        //{
        //    return wallCommand!;
        //}

        // Try reading pattern: <project name>
        if (TryParseReadingCommand(input, out var readCommand))
        {
            return readCommand!;
        }

        throw new ArgumentException("Invalid command format");
    }

    private static bool TryParsePostingCommand(string input, out CreatePostCommand? command)
    {
        command = null;
        
        // Pattern: <user name> -> @<project name> <message>
        var arrowIndex = input.IndexOf(" -> @", StringComparison.Ordinal);
        if (arrowIndex == -1)
        {
            return false;
        }

        var userName = input[..arrowIndex].Trim();
        var remaining = input[(arrowIndex + 5)..]; // Skip " -> @"
        
        var spaceIndex = remaining.IndexOf(' ');
        if (spaceIndex == -1)
        {
            return false;
        }

        var projectName = remaining[..spaceIndex].Trim();
        var message = remaining[(spaceIndex + 1)..].Trim();

        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(projectName) ||
            string.IsNullOrWhiteSpace(message))
        {
            return false;
        }

        command = new CreatePostCommand(DateTime.Now, userName, projectName, message);
        return true;
    }

    private static bool TryParseFollowingCommand(string input, out FollowProjectCommand? command)
    {
        command = null;

        // Pattern: <user name> follows <project name>
        var followsIndex = input.IndexOf(" follows ", StringComparison.Ordinal);
        if (followsIndex == -1)
        {
            return false;
        }

        var userName = input[..followsIndex].Trim();
        var projectName = input[(followsIndex + 9)..].Trim(); // Skip " follows "

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(projectName))
        {
            return false;
        }

        command = new FollowProjectCommand(userName, projectName);
        return true;
    }

    //private static bool TryParseWallCommand(string input, out GetUserWallQuery? command)
    //{
    //    command = null;

    //    // Pattern: <user name> wall
    //    if (!input.EndsWith(" wall", StringComparison.Ordinal)) return false;

    //    var userName = input[..^5].Trim(); // Remove " wall"
    //    if (string.IsNullOrWhiteSpace(userName)) return false;

    //    command = new GetUserWallQuery(userName);
    //    return true;
    //}

    private static bool TryParseReadingCommand(string input, out ReadProjectQuery? command)
    {
        command = null;

        // Pattern: <project name> (single word, no spaces)
        if (input.Contains(' ')) return false;

        var projectName = input.Trim();
        if (string.IsNullOrWhiteSpace(projectName)) return false;

        command = new ReadProjectQuery(projectName);
        return true;
    }

    public static void DisplaySuccessMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]✓[/] {message}");
    }

    public static void DisplayErrorMessage(string message)
    {
        AnsiConsole.MarkupLine($"[red]✗[/] {message}");
    }

    public static void DisplayInfoMessage(string message)
    {
        AnsiConsole.MarkupLine($"[blue]ℹ[/] {message}");
    }
}