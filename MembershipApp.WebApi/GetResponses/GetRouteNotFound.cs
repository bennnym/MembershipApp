using System.Net;

namespace MembershipApp.WebApi.GetResponses
{
    public class GetRouteNotFound : IApiGetResponse
    {
        public void ProcessRequest(HttpListenerResponse response, string queryParams = null)
        {
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(" ");
            response.ContentType = "application/json";
            response.StatusCode = 404;
            response.ContentLength64 = responseBuffer.Length;
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
        }

        public bool IsValid(string resourceName, string[] queryParams)
        {
            throw new System.NotImplementedException();
        }
    }
}