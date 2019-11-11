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


            Url = config["AppSettings:url"];
        }

        public string Url { get; private set; }
    }
}