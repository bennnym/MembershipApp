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
            var getQueryResponses = new List<IApiGetResponse>
            {
                new AllMembersQuery(), new SingleMemberQuery()
            };

            var deleteQueryResponses = new List<IApiDeleteResponse>
            {
                new DeleteMemberResponse()
            };

            var postQueryResponses = new List<IApiPostResponse>
            {
                new CreateNewMemberResponse()
            };

            var patchQueryResponses = new List<IApiPatchResponse>
            {
                new PatchResponse()
            };
            
            

            var apiGetResponseFactory = new ApiGetResponseFactory(getQueryResponses);
            var apiDeleteResponseFactory = new ApiDeleteResponseFactory(deleteQueryResponses);
            var apiPostResponseFactory = new ApiPostResponseFactory(postQueryResponses);
            var apiPatchResponseFactory = new ApiPatchResponseFactory(patchQueryResponses);
            
            var frameworkLessApp = new FrameworklessApi(apiGetResponseFactory, apiDeleteResponseFactory,
                apiPostResponseFactory, apiPatchResponseFactory);

            frameworkLessApp.Listen();
        }
    }
}