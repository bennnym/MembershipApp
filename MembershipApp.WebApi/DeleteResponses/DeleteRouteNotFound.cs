using System.Net;

namespace MembershipApp.WebApi.DeleteResponses
{
    public class DeleteRouteNotFound : IApiDeleteResponse
    {
        public void ProcessRequest(HttpListenerResponse response, string queryParams)
        {
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(" ");
            response.ContentType = "application/json";
            response.StatusCode = 404;
            response.ContentLength64 = responseBuffer.Length;
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
        }

        public bool IsValid(string resourceName, string queryParams)
        {
            throw new System.NotImplementedException();
        }
    }
}