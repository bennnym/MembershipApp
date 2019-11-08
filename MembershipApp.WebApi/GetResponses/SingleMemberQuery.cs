using System.Linq;
using System.Net;
using MembershipApp.Models;
using Newtonsoft.Json;

namespace MembershipApp.WebApi.GetResponses
{
    public class SingleMemberQuery : IApiGetResponse
    {
        private HttpStatusCode _statusCode;

        public void ProcessRequest(HttpListenerResponse response, string queryParams)
        {
            var memberData = MembershipManager.Instance.GetMember(int.Parse(queryParams));

            if (memberData != null)
            {
                _statusCode = HttpStatusCode.OK;
                var stringMemberData = JsonConvert.SerializeObject(memberData);
                ProcessResponse(response, stringMemberData);
            }
            else
            {
                _statusCode = HttpStatusCode.NotFound;
                ProcessResponse(response, " ");
            }

        }

        public bool IsValid(string resourceName, string[] queryParams)
        {
            return resourceName == "members" && queryParams.Any();
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