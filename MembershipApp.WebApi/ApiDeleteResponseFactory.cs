using System.Collections.Generic;
using MembershipApp.WebApi.DeleteResponses;
using MembershipApp.WebApi.GetResponses;

namespace MembershipApp.WebApi
{
    public class ApiDeleteResponseFactory
    {
        private readonly List<IApiDeleteResponse> _apiDeleteQueryAction;

        public ApiDeleteResponseFactory(List<IApiDeleteResponse> apiDeleteQueryAction)
        {
            _apiDeleteQueryAction = apiDeleteQueryAction;
        }
        public IApiDeleteResponse CreateDeleteResponseObject(string resourceName, string queryParams)
        {
            foreach (var apiDeleteAction in _apiDeleteQueryAction)
            {
                if (apiDeleteAction.IsValid(resourceName, queryParams))
                {
                    return apiDeleteAction;
                }
            }
   
            return new DeleteRouteNotFound();
        }
    }
}