using System.Collections.Generic;
using MembershipApp.WebApi.GetResponses;
using MembershipApp.WebApi.PostResponses;

namespace MembershipApp.WebApi
{
    public class ApiPostResponseFactory
    {
        private readonly List<IApiPostResponse> _apiPostQueryActions;

        public ApiPostResponseFactory(List<IApiPostResponse> apiPostQueryActions)
        {
            _apiPostQueryActions = apiPostQueryActions;
        }

        public IApiPostResponse CreatePostResponseObject(string resourceName, string[] queryParams)
        {
            foreach (var postQueryAction in _apiPostQueryActions)
            {
                if (postQueryAction.IsValid(resourceName, queryParams))
                {
                    return postQueryAction;
                }
            }
            
            return new PostRouteNotFound();
        }
    }
}