using System;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using MembershipApp.Tests.Integration.ClassFixtures;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class DeleteRequestIntegrationTests : IClassFixture<UrlFixture>, IClassFixture<MemberFixture>
    {
        private readonly Member _testMember;
        private readonly string _url;

        public DeleteRequestIntegrationTests(UrlFixture urlFixture, MemberFixture memberFixture)
        {
            _url = urlFixture.Url;
            _testMember = memberFixture.FirstTestMember;
        }
        
        [Fact]
        public async void Should_Delete_Member_When_Valid_Resource_And_Id_Is_Requested()
        {
            using (var postHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
            {
                //Arrange
                var testMemberString = JsonConvert.SerializeObject(_testMember);
                var postResponse = await postHttpClient.PostAsync("/members", new StringContent(testMemberString));
                var postResponseData = await postResponse.Content.ReadAsStringAsync();
                var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);

                // Act
                using (var deleteHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
                {
                    var response = await deleteHttpClient.DeleteAsync($"/members/{postResponseMember.Id}");
                    var responseData = await response.Content.ReadAsStringAsync();

                    using (var getHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
                    {
                        var getResponse = await getHttpClient.GetAsync($"/members/{postResponseMember.Id}");

                        // Assert
                        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
                        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                        Assert.Equal(" ", responseData);
                    }
                }
            }
            
        }

        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Trying_To_Delete_A_Member_That_Does_Not_Exist()
        {
            using (var deleteHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
            {
                // Act
                var deleteResponse = await deleteHttpClient.DeleteAsync($"/members/{int.MaxValue}");

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }
        
        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Making_A_Delete_Request_To_An_Invalid_Route()
        {
            using (var deleteHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
            {
                // Act
                var deleteResponse = await deleteHttpClient.DeleteAsync("/members/");

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
      
        }
    }
}