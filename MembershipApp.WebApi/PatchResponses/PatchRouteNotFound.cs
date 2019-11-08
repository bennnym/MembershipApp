using System.Net;

namespace MembershipApp.WebApi.PatchResponses
{
    public class PatchRouteNotFound : IApiPatchResponse
    {
        public void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response, string queryParams)
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