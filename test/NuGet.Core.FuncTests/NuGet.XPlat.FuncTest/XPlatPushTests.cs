// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.CommandLine.XPlat;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatPushTests
    {
        [Theory]
        [InlineData(TestServers.Artifactory, nameof(TestServers.Artifactory))]
        // [InlineData(TestServers.Klondike, nameof(TestServers.Klondike))] // 500 Internal Server Error pushing
        [InlineData(TestServers.MyGet, nameof(TestServers.MyGet))]
        [InlineData(TestServers.Nexus, nameof(TestServers.Nexus))] // can't push twice, but can delete
        // [InlineData(TestServers.NuGetServer, nameof(TestServers.NuGetServer))] // need api key
        [InlineData(TestServers.ProGet, nameof(TestServers.ProGet))] // can't delete
        public async Task PushToServerSucceeds(string sourceUri, string feedName)
        {
            // Arrange
            using (var packageDir = TestFileSystemUtility.CreateRandomTestFolder())
            {
                var packageId = "XPlatPushTests.PushToServerSucceeds";
                var packageVersion = "1.0.0";
                var packageFile = await TestPackages.GetRuntimePackageAsync(packageDir, packageId, packageVersion);
                var configFile = XPlatTestUtils.CopyFuncTestConfig(packageDir);
                var log = new TestCommandOutputLogger();

                var apiKey = XPlatTestUtils.ReadApiKey(feedName);
                Assert.False(string.IsNullOrEmpty(apiKey));

                var pushArgs = new List<string>()
                {
                    "push",
                    packageFile.FullName,
                    "--source",
                    sourceUri,
                    "--apikey",
                    apiKey
                };

                // Act
                int exitCode = Program.MainInternal(pushArgs.ToArray(), log);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains($@"OK {sourceUri}/FindPackagesById()?id='fody'", log.ShowMessages());
            }
        }
    }
}
