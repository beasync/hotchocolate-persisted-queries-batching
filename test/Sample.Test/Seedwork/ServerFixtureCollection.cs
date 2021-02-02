using Xunit;

namespace Sample.Test
{
    [CollectionDefinition(nameof(ServerFixtureCollection))]
    public class ServerFixtureCollection : ICollectionFixture<ServerFixture>
    {
    }
}
