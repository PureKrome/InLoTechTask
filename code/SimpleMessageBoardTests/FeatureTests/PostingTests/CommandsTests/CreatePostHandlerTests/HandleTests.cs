using SimpleMessageBoard.Features.Posting.Commands;
using SimpleMessageBoard.Models;
using SimpleMessageBoard.Repositories;

namespace SimpleMessageBoardTests.FeatureTests.PostingTests.CommandsTests.CreatePostHandlerTests;

// NOTE: PRIMARY A.I. GENERATED.
// HERE BE DRAGONS! 🐉🐉

public class HandleTests
{
    private readonly Mock<IRepository> _repository;
    private readonly CreatePostHandler _handler;

    public HandleTests()
    {
        _repository = new Mock<IRepository>();
        _handler = new CreatePostHandler(_repository.Object);
    }

    // Can add a post when we are following.
    [Fact]
    public async Task Handle_UserFollowingProject_ReturnsPostAdded()
    {
        // Arrange
        var command = new CreatePostCommand(
            PostedAt: DateTime.Now,
            UserName: "testuser",
            Project: "testproject",
            Message: "This is a test message"
        );

        _repository
            .Setup(x => x.AlreadyFollowingProject(command.UserName, command.Project))
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(HandlerResult.PostAdded);

        _repository.Verify(
            x => x.AlreadyFollowingProject(command.UserName, command.Project),
            Times.Once);

        _repository.Verify(
            x => x.AddPost(It.Is<Post>(p =>
                p.PostedAt == command.PostedAt &&
                p.UserName == command.UserName &&
                p.Message == command.Message),
                command.Project),
            Times.Once);
    }

    // Cannot add a post when we are not following.
    [Fact]
    public async Task Handle_UserNotFollowingProject_ReturnsNotFollowing()
    {
        // Arrange
        var command = new CreatePostCommand(
            PostedAt: DateTime.Now,
            UserName: "testuser",
            Project: "testproject",
            Message: "This is a test message"
        );

        _repository
            .Setup(x => x.AlreadyFollowingProject(command.UserName, command.Project))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(HandlerResult.NotFollowing);

        _repository.Verify(
            x => x.AlreadyFollowingProject(command.UserName, command.Project),
            Times.Once);

        _repository.Verify(
            x => x.AddPost(It.IsAny<Post>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UserFollowingProject_CreatesPostWithCorrectProperties()
    {
        // Arrange
        var postedAt = new DateTime(2023, 12, 25, 10, 30, 0);
        var command = new CreatePostCommand(
            PostedAt: postedAt,
            UserName: "johndoe",
            Project: "myproject",
            Message: "Hello world!"
        );

        Post capturedPost = null;
        string capturedProject = null;

        _repository
            .Setup(x => x.AlreadyFollowingProject(command.UserName, command.Project))
            .Returns(true);

        _repository
            .Setup(x => x.AddPost(It.IsAny<Post>(), It.IsAny<string>()))
            .Callback<Post, string>((post, project) =>
            {
                capturedPost = post;
                capturedProject = project;
            });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedPost.ShouldNotBeNull();
        capturedPost.PostedAt.ShouldBe(postedAt);
        capturedPost.UserName.ShouldBe("johndoe");
        capturedPost.Message.ShouldBe("Hello world!");
        capturedProject.ShouldBe("myproject");
    }

    [Theory]
    [InlineData("user1", "project1", "message1")]
    [InlineData("differentuser", "anotherproject", "different message")]
    [InlineData("", "", "")]
    public async Task Handle_VariousInputs_WhenFollowing_ReturnsPostAdded(
        string userName,
        string project, 
        string message)
    {
        // Arrange
        var command = new CreatePostCommand(
            PostedAt: DateTime.Now,
            UserName: userName,
            Project: project,
            Message: message
        );

        _repository
            .Setup(x => x.AlreadyFollowingProject(userName, project))
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(HandlerResult.PostAdded);
    }

    [Theory]
    [InlineData("user1", "project1", "message1")]
    [InlineData("differentuser", "anotherproject", "different message")]
    public async Task Handle_VariousInputs_WhenNotFollowing_ReturnsNotFollowing(
        string userName, 
        string project, 
        string message)
    {
        // Arrange
        var command = new CreatePostCommand(
            PostedAt: DateTime.Now,
            UserName: userName,
            Project: project,
            Message: message
        );

        _repository
            .Setup(x => x.AlreadyFollowingProject(userName, project))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(HandlerResult.NotFollowing);
    }
}
