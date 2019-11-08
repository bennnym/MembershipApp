using System.Net;

namespace MembershipApp.WebApi.PostResponses
{
    public interface IApiPostResponse
    {
        void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response);
        bool IsValid(string resourceName, string[] queryParams);
    }
}