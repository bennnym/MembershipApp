using System.Collections.Generic;
using MembershipApp.WebApi.DeleteResponses;
using MembershipApp.WebApi.GetResponses;
using MembershipApp.WebApi.PatchResponses;
using MembershipApp.WebApi.PostResponses;

namespace MembershipApp.WebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var getQueryActions = new List<IApiGetResponse>
            {
                new AllMembersQuery(), new SingleMemberQuery()
            };

            var deleteQueryActions = new List<IApiDeleteResponse>
            {
                new DeleteMemberResponse()
            };

            var postQueryActions = new List<IApiPostResponse>
            {
                new CreateNewMemberResponse()
            };

            var patchQueryActions = new List<IApiPatchResponse>
            {
                new PatchResponse()
            };

            var apiGetResponseFactory = new ApiGetResponseFactory(getQueryActions);
            var apiDeleteResponseFactory = new ApiDeleteResponseFactory(deleteQueryActions);
            var apiPostResponseFactory = new ApiPostResponseFactory(postQueryActions);
            var apiPatchResponseFactory = new ApiPatchResponseFactory(patchQueryActions);
            var frameworkLessApp = new FrameworklessApi(apiGetResponseFactory, apiDeleteResponseFactory,
                apiPostResponseFactory, apiPatchResponseFactory);

            frameworkLessApp.Listen();
        }
    }
}