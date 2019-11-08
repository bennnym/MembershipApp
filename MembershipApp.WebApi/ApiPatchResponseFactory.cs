using System.Collections.Generic;
using MembershipApp.WebApi.GetResponses;
using MembershipApp.WebApi.PatchResponses;

namespace MembershipApp.WebApi
{
    public class ApiPatchResponseFactory
    {
        private readonly List<IApiPatchResponse> _apiPostQueryActions;
        
        public ApiPatchResponseFactory(List<IApiPatchResponse> apiPostQueryActions)
        {
            _apiPostQueryActions = apiPostQueryActions;
        }

        public IApiPatchResponse CreatePatchResponseObject(string resourceName, string[] queryParams)
        {
            foreach (var postQueryAction in _apiPostQueryActions)
            {
                if (postQueryAction.IsValid(resourceName, queryParams))
                {
                    return postQueryAction;
                }
            }
            
            return new PatchRouteNotFound();
        }
    }
}