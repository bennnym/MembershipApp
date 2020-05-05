using Microsoft.Extensions.Configuration;

namespace MembershipApp.Tests.Integration.ClassFixtures
{
    public class UrlFixture
    {
        public UrlFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            Url = config["AppSettings:prod"];
        }

        public string Url { get; }
    }
}