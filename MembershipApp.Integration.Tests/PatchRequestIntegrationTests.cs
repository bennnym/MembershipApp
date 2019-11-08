using System;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class PatchRequestIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly Member _testMember;

        public PatchRequestIntegrationTests()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:8080/")};
            _testMember = new Member
            {
                FirstName = "Ben",
                LastName = "Muller"
            };
   
        }
        [Fact]
        public async void Should_Update_Users_First_Name_When_A_Patch_Update_Is_Made()
        {
            //Arrange
            var testMemberString = JsonConvert.SerializeObject(_testMember);
            var postResponse = await _httpClient.PostAsync("/members", new StringContent(testMemberString));
            var postResponseData = await postResponse.Content.ReadAsStringAsync();
            var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);

            // Act
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080");
                var updatedFirstNameMember = new Member
                {
                    FirstName = "James"
                };
                var memberUpdateAsString = JsonConvert.SerializeObject(updatedFirstNameMember);
                var patchResponse = await client.PatchAsync($"/members/{postResponseMember.Id}",
                    new StringContent(memberUpdateAsString));
                var patchResponseData = await patchResponse.Content.ReadAsStringAsync();
                var updatedMember = JsonConvert.DeserializeObject<Member>(patchResponseData);

                // Assert
                Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);
                Assert.Equal(updatedFirstNameMember.FirstName, updatedMember.FirstName);
            }
        }
        
        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Trying_To_Patch_A_Member_That_Does_Not_Exist()
        {
            // Arrange
            var dummyString = "fake data";
            // Act
            var patchResponse = await _httpClient.PatchAsync($"/members/{int.MaxValue}", new StringContent(dummyString));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, patchResponse.StatusCode);
        }
    }
}