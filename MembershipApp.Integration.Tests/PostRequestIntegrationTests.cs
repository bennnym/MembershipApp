using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using MembershipApp.Tests.Integration.ClassFixtures;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class PostRequestIntegrationTests : IClassFixture<UrlFixture>, IClassFixture<MemberFixture>
    {
        private readonly Member _testMember;
        private readonly string _url;

        public PostRequestIntegrationTests(UrlFixture urlFixture, MemberFixture memberFixture)
        {
            _url = urlFixture.Url;
            _testMember = memberFixture.FirstTestMember;
        }

        [Fact]
        public async void Should_Add_A_New_Member_When_A_Valid_Post_Request_Is_Made()
        {
            using (var postHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
            {
                // Arrange
                var testMemberString = JsonConvert.SerializeObject(_testMember);

                // Act
                var response = await postHttpClient.PostAsync("/members", new StringContent(testMemberString));
                var responseData = await response.Content.ReadAsStringAsync();
                var responseMember = JsonConvert.DeserializeObject<Member>(responseData);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Ben", responseMember.FirstName);
                Assert.Equal("Muller", responseMember.LastName);
            }
       
        }

        [Fact]
        public async void Should_Return_A_NotFound_Status_Code_When_An_Invalid_Post_Route_Is_Requested()
        {
            using (var postHttpClient = new HttpClient{BaseAddress = new Uri(_url)})
            {
                // Arrange
                var dummyString = "fake data";

                // Act
                var response = await postHttpClient.PostAsync("/wrongRoute", new StringContent(dummyString));

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
          
        }

       
       


      
    }
}