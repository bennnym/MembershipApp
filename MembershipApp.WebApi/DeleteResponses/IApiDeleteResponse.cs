using System.Net;

namespace MembershipApp.WebApi.DeleteResponses
{
    public interface IApiDeleteResponse
    {
        void ProcessRequest(HttpListenerResponse response, string queryParams);
        bool IsValid(string resourceName, string queryParams);
    }
}