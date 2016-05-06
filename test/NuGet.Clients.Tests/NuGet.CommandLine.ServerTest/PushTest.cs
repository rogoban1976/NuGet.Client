using Xunit;

namespace NuGet.CommandLine.ServerTest
{
    public class PushTest
    {
        [Theory]
        [InlineData(TestServers.ServerType.Artifactory)]
        [InlineData(TestServers.ServerType.Klondike)]
        [InlineData(TestServers.ServerType.MyGet)]
        [InlineData(TestServers.ServerType.Nexus)]
        [InlineData(TestServers.ServerType.NuGetServer)]
        [InlineData(TestServers.ServerType.Proget)]
        [InlineData(TestServers.ServerType.Vsts)]
        public void PushToServer(TestServers.ServerType serverType)
        {
            TestServerInfo info = TestServers.GetServerInfo(serverType);
            Assert.NotNull(info);
        }
    }
}
