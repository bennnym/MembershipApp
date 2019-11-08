using System;
using System.IO;
using System.Linq;
using System.Net;
using MembershipApp.Models;
using Newtonsoft.Json;

namespace MembershipApp.WebApi.PostResponses
{
    public class CreateNewMemberResponse : IApiPostResponse
    {
        private HttpStatusCode _statusCode;

        public void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            var body = request.InputStream;
            var encoding = request.ContentEncoding;
            var reader = new StreamReader(body, encoding);
            var requestBody = reader.ReadToEnd();

            var memberToCreate = JsonConvert.DeserializeObject<Member>(requestBody);
            
            body.Close();
            reader.Close();

            if (HasFirstAndLastName(memberToCreate))
            {
                var newMemberString = CreateNewMemberFromRawString(memberToCreate);
                _statusCode = HttpStatusCode.OK;
                ProcessResponse(response, newMemberString);
            }
            else
            {
                _statusCode = HttpStatusCode.NotFound;
                ProcessResponse(response, " ");
            }
        }
        
                
        public bool IsValid(string resourceName, string[] queryParams)
        {
            return resourceName == "members" && !queryParams.Any();
        }

        private string CreateNewMemberFromRawString(IMember member)
        {
            var newMember = MembershipManager.Instance.CreateNewMember(member.FirstName, member.LastName);
            return JsonConvert.SerializeObject(newMember);
        }

        private void ProcessResponse(HttpListenerResponse response, string responseMessage = null)
        {
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseMessage);
            response.ContentType = "application/json";
            response.StatusCode = (int) _statusCode;
            response.ContentLength64 = responseBuffer.Length;
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length); // forces send of response
        }

        private bool HasFirstAndLastName(IMember member)
        {
            return member.FirstName != null && member.LastName != null;
        }
    }
}