using System;
using System.IO;
using System.Xml;
using NuGet.Common;

namespace NuGet.CommandLine.ServerTest
{
    /// <summary>
    /// Provides a TestServer object for each known test server that supports NuGet
    /// </summary>
    public static class TestServers
    {
        public enum ServerType
        {
            None,
            Artifactory,
            Klondike,
            MyGet,
            Nexus,
            NuGetServer,
            Proget,
            TeamCity,
            Vsts,
        }

        public static TestServerInfo GetServerInfo(ServerType name)
        {
            string url = null;
            bool hasApikey = false;
            bool hasPassword = false;
            TestServerInfo.BehaviorTypes behaviors = TestServerInfo.BehaviorTypes.None;

            switch (name)
            {
                case ServerType.Artifactory:
                    url = @"http://artifactory:8081/artifactory/api/nuget/nuget";
                    break;

                case ServerType.Klondike:
                    url = @"http://klondikeserver:8081/api/odata";
                    break;

                case ServerType.MyGet:
                    url = @"https://www.myget.org/F/myget-server-test/api/v2";
                    break;

                case ServerType.Nexus:
                    url = @"http://nexusservertest:8081/nexus/service/local/nuget/NuGet";
                    break;

                case ServerType.NuGetServer:
                    url = @"http://nugetserverendpoint.azurewebsites.net/nuget";
                    hasPassword = true;
                    break;

                case ServerType.Proget:
                    url = @"http://progetserver:8081/nuget/nuget";
                    break;

                case ServerType.TeamCity:
                    url = @"http://teamcityserver:8081/guestAuth/app/nuget/v1/FeedService.svc";
                    break;

                case ServerType.Vsts:
                    url = "https://vstsnugettest.pkgs.visualstudio.com/DefaultCollection/_packaging/VstsTestFeed/nuget/v2";
                    hasPassword = true;
                    break;

                default:
                    throw new ArgumentException("Unknown name", nameof(name));
            }

            if (hasPassword)
            {
                return new TestServerInfo(url, LoadNameAndPassword(name), behaviors);
            }

            if (hasApikey)
            {
                return new TestServerInfo(url, LoadApiKey(name), behaviors);
            }

            return new TestServerInfo(url, behaviors);
        }

        private static string LoadApiKey(ServerType name)
        {
            XmlDocument doc = LoadTestConfig();
            return doc.SelectSingleNode($@"/configuration/{name}/ApiKey").InnerText;
        }

        private static Tuple<string, string> LoadNameAndPassword(ServerType name)
        {
            XmlDocument doc = LoadTestConfig();
            string userName = doc.SelectSingleNode($@"/configuration/{name}/Username").InnerText;
            string password = doc.SelectSingleNode($@"/configuration/{name}/Password").InnerText;

            return new Tuple<string, string>(userName, password);
        }

        private static XmlDocument LoadTestConfig()
        {
            string dir = NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);

            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(dir, "test.config"));

            return doc;
        }
    }
}
