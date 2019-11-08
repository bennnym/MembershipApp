using System.Net;

namespace MembershipApp.WebApi.PatchResponses
{
    public interface IApiPatchResponse
    {
        void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response, string queryParams);
        bool IsValid(string resourceName, string[] queryParams);
    }
}