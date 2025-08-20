using SimpleMessageBoard.Repositories;

namespace SimpleMessageBoardTests.RepositoriesTests.InMemoryRepositoryTests;

public class FollowProjectTests
{
    public static TheoryData<string, string> GetFollowProjectTestData() => new TheoryData<string, string>
    {
        // Single user + single project
        { "alice", "moonshot" },
        { "bob", "webapp" },

        // Multiple users + single project
        { "charlie", "ai-project" },
        { "diana", "ai-project" },
        { "eve", "ai-project" },

        // Multiple users + multiple projects
        { "frank", "mobile-app" },
        { "grace", "desktop-tool" },
        { "henry", "game-engine" },
        { "iris", "data-pipeline" },
        { "jack", "blockchain-wallet" }
    };

    [Theory]
    [MemberData(nameof(GetFollowProjectTestData))]
    public void FollowProject_GivenAValidProjectName_ShouldFollowProject(string userName, string project)
    {
        // Arrange.
        var repository = new InMemoryRepository();

        // Act.
        repository.FollowProject(userName, project);

        // Assert.
    }
}
