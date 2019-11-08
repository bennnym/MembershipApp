using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class PostRequestIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly Member _testMember;

        public PostRequestIntegrationTests()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:8080/")};
            _testMember = new Member
            {
                FirstName = "Ben",
                LastName = "Muller"
            };
        }

        [Fact]
        public async void Should_Add_A_New_Member_When_A_Valid_Post_Request_Is_Made()
        {
            // Arrange
            var testMemberString = JsonConvert.SerializeObject(_testMember);

            // Act
            var response = await _httpClient.PostAsync("/members", new StringContent(testMemberString));
            var responseData = await response.Content.ReadAsStringAsync();
            var responseMember = JsonConvert.DeserializeObject<Member>(responseData);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Ben", responseMember.FirstName);
            Assert.Equal("Muller", responseMember.LastName);
        }
        

        [Fact]
        public async void Should_Return_A_NotFound_Status_Code_When_An_Invalid_Post_Route_Is_Requested()
        {
            // Arrange
            var dummyString = "fake data";

            // Act
            var response = await _httpClient.PostAsync("/wrongRoute", new StringContent(dummyString));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

       
       


      
    }
}