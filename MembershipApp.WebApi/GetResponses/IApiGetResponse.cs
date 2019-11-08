using System.Net;

namespace MembershipApp.WebApi.GetResponses
{
    public interface IApiGetResponse
    {
        void ProcessRequest(HttpListenerResponse response, string queryParams = null);
        bool IsValid(string resourceName, string[] queryParams);
    }
}