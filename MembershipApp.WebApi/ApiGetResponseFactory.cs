using System.Collections.Generic;
using MembershipApp.WebApi.GetResponses;

namespace MembershipApp.WebApi
{
    public class ApiGetResponseFactory
    {
        private readonly List<IApiGetResponse> _apiGetQueryActions;

        public ApiGetResponseFactory(List<IApiGetResponse> apiGetQueryActions)
        {
            _apiGetQueryActions = apiGetQueryActions;
        }
        public  IApiGetResponse CreateGetResponseObject(string resourceName, string[] queryParams)
        {
            foreach (var getQueryAction in _apiGetQueryActions)
            {
                if (getQueryAction.IsValid(resourceName, queryParams))
                {
                    return getQueryAction;
                }
            }
            
            return new GetRouteNotFound();
        }

    }
}