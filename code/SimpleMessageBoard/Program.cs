using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleMessageBoard.Features.Following.Commands;
using SimpleMessageBoard.Features.Posting.Commands;
using SimpleMessageBoard.Repositories;
using SimpleMessageBoard.Services;

var builder = Host.CreateApplicationBuilder(args);

// Register MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// Register dependencies
builder.Services.AddSingleton<IRepository, InMemoryRepository>();

var host = builder.Build();

var mediator = host.Services.GetRequiredService<IMediator>();

Console.WriteLine("Simple Message Board");
Console.WriteLine("====================");
Console.WriteLine("Commands:");
Console.WriteLine("  Posting: <user name> -> @<project name> <message>");
Console.WriteLine("  Reading: <project name>");
Console.WriteLine("  Following: <user name> follows <project name>");
Console.WriteLine("  Wall: <user name> wall");
Console.WriteLine("  Type 'exit' to quit");
Console.WriteLine();

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    try
    {
        var parser = new SpectreCommandParser();
        var command = parser.ParseCommand(input);

        // Handle different command types
        if (command is CreatePostCommand)
        {
            await mediator.Send(command);
            SpectreCommandParser.DisplaySuccessMessage("Post created successfully!");
        }
        else if (command is FollowProjectCommand followCommand)
        {
            var success = await mediator.Send(followCommand);
            if (success)
            {
                SpectreCommandParser.DisplaySuccessMessage($"Successfully followed project {followCommand.ProjectName}");
            }
            else
            {
                SpectreCommandParser.DisplayInfoMessage($"Already following project {followCommand.ProjectName}");
            }
        }
        else
        {
            await mediator.Send(command);
            SpectreCommandParser.DisplaySuccessMessage("Command executed successfully.");
        }
    }
    catch (Exception ex)
    {
        SpectreCommandParser.DisplayErrorMessage($"Error: {ex.Message}");
    }

    Console.WriteLine();
}