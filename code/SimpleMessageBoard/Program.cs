using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleMessageBoard.Features.Following.Commands;
using SimpleMessageBoard.Features.Posting.Commands;
using SimpleMessageBoard.Features.Reading.Queries;
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

//Console.WriteLine("Simple Message Board");
//Console.WriteLine("====================");
//Console.WriteLine("Commands:");
//Console.WriteLine("  Posting: <user name> -> @<project name> <message>");
//Console.WriteLine("  Reading: <project name>");
//Console.WriteLine("  Following: <user name> follows <project name>");
//Console.WriteLine("  Wall: <user name> wall");
//Console.WriteLine("  Type 'exit' to quit");
//Console.WriteLine();

SpectreDisplayService.DisplayWelcomeMessage();

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
        if (command is CreatePostCommand createPostCommand)
        {
            var result = await mediator.Send(createPostCommand);
            if (result == SimpleMessageBoard.Features.Posting.Commands.HandlerResult.NotFollowing)
            {
                SpectreCommandParser.DisplayErrorMessage("You must follow the project before posting.");
                continue;
            }
            else
            {
                SpectreCommandParser.DisplaySuccessMessage("Post created successfully!");
            }
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
        else if (command is ReadProjectQuery readCommand)
        {
            var result = await mediator.Send(readCommand);
            result.Switch(
                handlerResult =>
                {
                    if (handlerResult == SimpleMessageBoard.Features.Reading.Queries.HandlerResult.ProjectDoesntExist)
                    {
                        SpectreCommandParser.DisplayErrorMessage("Project doesn't exist.");
                    }
                    else
                    {
                        SpectreCommandParser.DisplayErrorMessage("Failed to retrieve project posts.");
                    }
                },
                posts =>
                {
                    if (posts.Count == 0)
                    {
                        SpectreCommandParser.DisplayInfoMessage("No posts found for this project.");
                    }
                    else
                    {
                        SpectreDisplayService.DisplayPosts(posts);
                        //Console.WriteLine($"Posts for project '{readCommand.project}':");
                        //foreach (var post in posts)
                        //{
                        //    Console.WriteLine($"  [{post.PostedAt:yyyy-MM-dd HH:mm:ss}] {post.UserName}: {post.Message}");
                        //}
                    }
                }
            );
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