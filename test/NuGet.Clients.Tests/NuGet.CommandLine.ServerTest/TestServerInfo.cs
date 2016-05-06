using System;

namespace NuGet.CommandLine.ServerTest
{
    public class TestServerInfo
    {
        [Flags]
        public enum BehaviorTypes
        {
            None = 0,
        }

        public string Url { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string ApiKey { get; private set; }
        public BehaviorTypes Behaviors { get; private set; }

        public TestServerInfo(string url, BehaviorTypes behaviors)
        {
            Url = url;
        }

        public TestServerInfo(string url, string userName, string password, BehaviorTypes behaviors)
        {
            Url = url;
            UserName = userName;
            Password = password;
        }

        public TestServerInfo(string url, Tuple<string, string> nameAndPassword, BehaviorTypes behaviors)
        {
            Url = url;
            UserName = nameAndPassword.Item1;
            Password = nameAndPassword.Item2;
        }

        public TestServerInfo(string url, string apiKey, BehaviorTypes behaviors)
        {
            Url = url;
            ApiKey = apiKey;
        }

        public bool HasApiKey
        {
            get { return !string.IsNullOrEmpty(ApiKey); }
        }

        public bool HasNameAndPassword
        {
            get { return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password); }
        }

        public bool HasBehavior(BehaviorTypes behavior)
        {
            return (Behaviors & behavior) == behavior;
        }
    }
}
