using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MembershipApp.Models;
using MembershipApp.Tests.Integration.ClassFixtures;
using Newtonsoft.Json;
using Xunit;

namespace MembershipApp.Tests.Integration
{
    public class GetRequestIntegrationTests : IClassFixture<UrlFixture>, IClassFixture<MemberFixture>
    {
        private readonly Member _testMember;
        private readonly Member _secondTestMember;
        private readonly string _url;

        public GetRequestIntegrationTests(UrlFixture urlFixture, MemberFixture memberFixture)
        {
            _url = urlFixture.Url;

            _testMember = memberFixture.FirstTestMember;
            _secondTestMember = memberFixture.SecondTestMember;
        }

        [Fact]
        public async void Should_Get_Data_Of_Member_When_Valid_Resource_And_Id_Is_Requested()
        {
            using (var postHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
            {
                //Arrange
                var testMemberString = JsonConvert.SerializeObject(_testMember);
                var postResponse = await postHttpClient.PostAsync("/members", new StringContent(testMemberString));
                var postResponseData = await postResponse.Content.ReadAsStringAsync();
                var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);


                // Act
                using (var getHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
                {
                    var response = await getHttpClient.GetAsync($"/members/{postResponseMember.Id}/");
                    var responseData = await response.Content.ReadAsStringAsync();
                    var responseMember = JsonConvert.DeserializeObject<Member>(responseData);

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(postResponseMember.Id, responseMember.Id);
                    Assert.Equal(postResponseMember.FirstName, responseMember.FirstName);
                    Assert.Equal(postResponseMember.LastName, responseMember.LastName);
                    Assert.Equal(postResponseMember.FullName, responseMember.FullName);
                }
            }
        }

        [Fact]
        public async void Should_Get_All_Members_When_Valid_All_Members_Get_Request_Is_Made()
        {
            using (var postHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
            {
                //Arrange
                var testMemberString = JsonConvert.SerializeObject(_testMember);
                var postResponse = await postHttpClient.PostAsync("/members", new StringContent(testMemberString));
                var postResponseData = await postResponse.Content.ReadAsStringAsync();
                var postResponseMember = JsonConvert.DeserializeObject<Member>(postResponseData);

                using (var secondPostHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
                {
                    var secondTestMemberString = JsonConvert.SerializeObject(_secondTestMember);
                    var otherPostResponse =
                        await secondPostHttpClient.PostAsync("/members", new StringContent(secondTestMemberString));
                    var otherPostResponseData = await otherPostResponse.Content.ReadAsStringAsync();
                    var otherPostResponseMember = JsonConvert.DeserializeObject<Member>(otherPostResponseData);

                    // Act
                    using (var getHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
                    {
                        var response = await getHttpClient.GetAsync("/members/");
                        var responseData = await response.Content.ReadAsStringAsync();
                        var responseMembersList = JsonConvert.DeserializeObject<List<Member>>(responseData);

                        var firstMember = responseMembersList.Find(m => m.Id == postResponseMember.Id);
                        var secondMember = responseMembersList.Find(m => m.Id == otherPostResponseMember.Id);

                        // Assert
                        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                        Assert.Equal(postResponseMember.Id, firstMember.Id);
                        Assert.Equal(postResponseMember.FirstName, firstMember.FirstName);
                        Assert.Equal(postResponseMember.LastName, firstMember.LastName);
                        Assert.Equal(postResponseMember.FullName, firstMember.FullName);

                        Assert.Equal(otherPostResponseMember.Id, secondMember.Id);
                        Assert.Equal(otherPostResponseMember.FirstName, secondMember.FirstName);
                        Assert.Equal(otherPostResponseMember.LastName, secondMember.LastName);
                        Assert.Equal(otherPostResponseMember.FullName, secondMember.FullName);
                    }


//                    Assert.Contains(postResponseMember,responseMembersList); // TODO: ASK FOR HELP ON THESE - Why can't I do this? - there has to be an easier way!!!!
//                    Assert.Contains(otherPostResponseMember, responseMembersList); // TODO: ASK FOR HELP ON THESE
                }
            }
        }

        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Get_Response_Is_Made_To_Valid_Route_And_Member_Does_Not_Exist()
        {
            using (var getHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
            {
                // Act
                var response = await getHttpClient.GetAsync($"/members/{Int32.MaxValue}");
                var responseData = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.Equal(" ", responseData);
            }
        }


        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Get_Response_Is_Made_To_Invalid_Route()
        {
            using (var getHttpClient = new HttpClient {BaseAddress = new Uri(_url)})
            {
                // Act
                var response = await getHttpClient.GetAsync("/wrongRoute");
                var responseData = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.Equal(" ", responseData);
            }
        }
    }
}