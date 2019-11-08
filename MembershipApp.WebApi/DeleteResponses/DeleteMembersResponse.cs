using System.Linq;
using System.Net;

namespace MembershipApp.WebApi.DeleteResponses
{
    class DeleteMemberResponse : IApiDeleteResponse
    {
        private HttpStatusCode _statusCode;

        public void ProcessRequest(HttpListenerResponse response, string queryParams)
        {
            var memberId = int.Parse(queryParams);
            var memberToDelete = MembershipManager.Instance.GetMember(memberId);

            if (memberToDelete != null)
            {
                _statusCode = HttpStatusCode.OK;
                MembershipManager.Instance.DeleteMember(memberId);
            }
            else
            {
                _statusCode = HttpStatusCode.NotFound;
            }
            
            ProcessResponse(response, " ");
        }

        public bool IsValid(string resourceName, string queryParams)
        {
            return resourceName == "members" && queryParams.Any();
        }

        private void ProcessResponse(HttpListenerResponse response, string responseMessage)
        {
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseMessage);
            response.ContentType = "application/json";
            response.StatusCode = (int) _statusCode;
            response.ContentLength64 = responseBuffer.Length;
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length); // forces send of response
        }
        
    }
}