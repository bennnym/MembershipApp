using System.Collections.Generic;
using System.Linq;
using System.Net;
using MembershipApp.Models;
using Newtonsoft.Json;

namespace MembershipApp.WebApi.GetResponses
{
    class AllMembersQuery : IApiGetResponse
    {
        private HttpStatusCode _statusCode;

        public void ProcessRequest(HttpListenerResponse response, string queryParams)
        {
            var allMembers = MembershipManager.Instance.GetAllMembers();
            var allMembersAsString = JsonConvert.SerializeObject(allMembers);

            SetStatusCode(allMembers);

            ProcessResponse(response, allMembersAsString);
        }

        public bool IsValid(string resourceName, string[] queryParams)
        {
            return resourceName == "members" && !queryParams.Any();
        }

        private void SetStatusCode(List<IMember> members)
        {
            if (members.Any())
            {
                _statusCode = HttpStatusCode.OK;
            }
            else
            {
                _statusCode = HttpStatusCode.NotFound;
            }
        }
        
        private void ProcessResponse(HttpListenerResponse response, string responseMessage = null)
        {
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseMessage);
            response.ContentType = "application/json";
            response.StatusCode = (int) _statusCode;
            response.ContentLength64 = responseBuffer.Length;
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length); // forces send of response
        }
    }
}