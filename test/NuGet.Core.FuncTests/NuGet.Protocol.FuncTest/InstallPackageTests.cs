using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.FuncTest
{
    public class InstallPackageTests
    {
        [Theory]
        [InlineData(TestServers.Nexus)]
        [InlineData(TestServers.ProGet)]
        [InlineData(TestServers.Klondike)]
        [InlineData(TestServers.Artifactory)]
        [InlineData(TestServers.MyGet)]
        public async Task InstallPackage_1(string packageSource)
        {
            using (var tempDir = TestFileSystemUtility.CreateRandomTestFolder())
            {
                // Arrange
                var source = new PackageSource(packageSource);
                var packageManager = new NuGetPackageManager(sourceRepositoryProvider, Settings, installPath);

                // Act
                await packageManager.InstallPackageAsync(
                    folderProject,
                    packageIdentity,
                    resolutionContext,
                    projectContext,
                    primaryRepositories,
                    Enumerable.Empty<SourceRepository>(),
                    CancellationToken.None);

                // Assert
            }
        }

        [Theory]
        [InlineData(TestServers.NuGetServer, "NuGetServer")]
        [InlineData(TestServers.Vsts,"Vsts")]
        public async Task InstallPackage_2(string packageSource, string feedName)
        {
#if false
            // Arrange
            var credential = Utility.ReadCredential(feedName);
            var source = new PackageSource(packageSource);
            source.UserName = credential.Item1;
            source.PasswordText = credential.Item2;
            source.IsPasswordClearText = true;
            var repo = Repository.Factory.GetCoreV2(source);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var context = new SourceCacheContext();
            context.NoCache = true;
            findPackageByIdResource.CacheContext = context;

            // Act
            var packages = await findPackageByIdResource.GetAllVersionsAsync("Newtonsoft.json", CancellationToken.None);

            // Assert
            Assert.Equal(1, packages.Count());
            Assert.Equal("8.0.3", packages.FirstOrDefault().ToString());
#endif
        }
    }
}
