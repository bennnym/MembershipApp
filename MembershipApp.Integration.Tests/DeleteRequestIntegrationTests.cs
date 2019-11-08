using System;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class DeleteRequestIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly Member _testMember;

        public DeleteRequestIntegrationTests()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:8080/")};
            _testMember = new Member
            {
                FirstName = "Ben",
                LastName = "Muller"
            };
        }
        [Fact]
        public async void Should_Delete_Member_When_Valid_Resource_And_Id_Is_Requested()
        {
            //Arrange
            var testMemberString = JsonConvert.SerializeObject(_testMember);
            var postResponse = await _httpClient.PostAsync("/members", new StringContent(testMemberString));
            var postResponseData = await postResponse.Content.ReadAsStringAsync();
            var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/");
                var response = await client.DeleteAsync($"/members/{postResponseMember.Id}");
                var responseData = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(" ", responseData);

                // do another get request and confirm with a 404
            }
        }

        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Trying_To_Delete_A_Member_That_Does_Not_Exist()
        {
            // Act
            var deleteResponse = await _httpClient.DeleteAsync($"/members/{int.MaxValue}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
        
        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Making_A_Delete_Request_To_An_Invalid_Route()
        {
            // Act
            var deleteResponse = await _httpClient.DeleteAsync("/members/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
    }
}