using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class GetRequestIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly Member _testMember;
        private readonly Member _secondTestMember;

        public GetRequestIntegrationTests()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:8080/")};
            _testMember = new Member
            {
                FirstName = "Ben",
                LastName = "Muller"
            };
            _secondTestMember = new Member
            {
                FirstName = "Leah",
                LastName = "Hou"
            };
        }

        [Fact]
        public async void Should_Get_Data_Of_Member_When_Valid_Resource_And_Id_Is_Requested()
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
                var response = await client.GetAsync($"/members/{postResponseMember.Id}/");
                var responseData = await response.Content.ReadAsStringAsync();
                var responseMember = JsonConvert.DeserializeObject<Member>(responseData);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(postResponseMember.Id, responseMember.Id);
                Assert.Equal(postResponseMember.FirstName, responseMember.FirstName);
                Assert.Equal(postResponseMember.LastName, responseMember.LastName);
            }
        }

        [Fact]
        public async void Should_Get_All_Members_When_Valid_All_Members_Get_Request_Is_Made()
        {
            //Arrange
            var testMemberString = JsonConvert.SerializeObject(_testMember);
            var postResponse = await _httpClient.PostAsync("/members", new StringContent(testMemberString));
            var postResponseData = await postResponse.Content.ReadAsStringAsync();
            var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/");
                var secondTestMemberString = JsonConvert.SerializeObject(_secondTestMember);
                var otherPostResponse = await client.PostAsync("/members", new StringContent(secondTestMemberString));
                var otherPostResponseData = await otherPostResponse.Content.ReadAsStringAsync();
                var otherPostResponseMember = JsonConvert.DeserializeObject<Member>(otherPostResponseData);

                // Act
                using (var nextClient = new HttpClient())
                {
                    nextClient.BaseAddress = new Uri("http://localhost:8080/");
                    var response = await nextClient.GetAsync("/members/");
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseMembersList = JsonConvert.DeserializeObject<List<Member>>(responseData);

                    var firstMember = responseMembersList.Find(m => m.Id == postResponseMember.Id);
                    var secondMember = responseMembersList.Find(m => m.Id == otherPostResponseMember.Id);

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(postResponseMember.Id, firstMember.Id);
                    Assert.Equal(postResponseMember.FirstName, firstMember.FirstName);
                    Assert.Equal(postResponseMember.LastName, firstMember.LastName);

                    Assert.Equal(otherPostResponseMember.Id, secondMember.Id);
                    Assert.Equal(otherPostResponseMember.FirstName, secondMember.FirstName);
                    Assert.Equal(otherPostResponseMember.LastName, secondMember.LastName);

//                    Assert.Contains(postResponseMember,responseMembersList); // TODO: ASK FOR HELP ON THESE - Why can't I do this? - there has to be an easier way!!!!
//                    Assert.Contains(otherPostResponseMember, responseMembersList); // TODO: ASK FOR HELP ON THESE
                }
            }
        }


        [Fact]
        public async void
            Should_Return_NotFound_Status_Code_When_Get_Response_Is_Made_To_Valid_Route_And_Member_Does_Not_Exist()
        {
            // Act
            var response = await _httpClient.DeleteAsync($"/members/{Int32.MaxValue}");
            var responseData = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(" ", responseData);
        }


        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Get_Response_Is_Made_To_Invalid_Route()
        {
            // Act
            var response = await _httpClient.GetAsync("/wrongRoute");
            var responseData = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(" ", responseData);
        }
    }
}