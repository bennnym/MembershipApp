using System;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using MembershipApp.Tests.Integration.ClassFixtures;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class PatchRequestIntegrationTests : IClassFixture<UrlFixture>, IClassFixture<MemberFixture>
    {
        private readonly Member _testMember;
        private readonly string _url;

        public PatchRequestIntegrationTests(UrlFixture urlFixture, MemberFixture memberFixture)
        {
            _url = urlFixture.Url;
            _testMember = memberFixture.FirstTestMember;
        }

        [Fact]
        public async void Should_Update_Users_First_Name_When_A_Patch_Update_Is_Made()
        {
            using (var postHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
            {
                //Arrange
                var testMemberString = JsonConvert.SerializeObject(_testMember);
                var postResponse = await postHttpClient.PostAsync("/members", new StringContent(testMemberString));
                var postResponseData = await postResponse.Content.ReadAsStringAsync();
                var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);

                // Act
                using (var patchHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
                {
                    var updatedFirstNameMember = new Member
                    {
                        FirstName = "James"
                    };
                    var memberUpdateAsString = JsonConvert.SerializeObject(updatedFirstNameMember);
                    var patchResponse = await patchHttpClient.PatchAsync($"/members/{postResponseMember.Id}",
                        new StringContent(memberUpdateAsString));
                    var patchResponseData = await patchResponse.Content.ReadAsStringAsync();
                    var updatedMember = JsonConvert.DeserializeObject<Member>(patchResponseData);

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);
                    Assert.Equal(updatedFirstNameMember.FirstName, updatedMember.FirstName);
                }
            }
        }

        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Trying_To_Patch_A_Member_That_Does_Not_Exist()
        {
            using (var patchHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
            {
                // Arrange
                var dummyString = "fake data";
                // Act
                var patchResponse =
                    await patchHttpClient.PatchAsync($"/members/{int.MaxValue}", new StringContent(dummyString));

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, patchResponse.StatusCode);
            }
        }
    }
}