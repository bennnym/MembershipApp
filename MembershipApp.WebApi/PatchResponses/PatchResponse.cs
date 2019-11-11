using System.IO;
using System.Linq;
using System.Net;
using MembershipApp.Models;
using Newtonsoft.Json;

namespace MembershipApp.WebApi.PatchResponses
{
    public class PatchResponse : IApiPatchResponse
    {
        private HttpStatusCode _statusCode;
        public void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response, string queryParams)
        {
            var body = request.InputStream;
            var encoding = request.ContentEncoding;
            var reader = new StreamReader(body, encoding);
            var requestBody = reader.ReadToEnd();
            body.Close();
            reader.Close();
            
            var memberToEdit = MembershipManager.Instance.GetMember(int.Parse(queryParams));

            if (memberToEdit != null)
            {
                var memberFieldToEdit = JsonConvert.DeserializeObject<Member>(requestBody);
                memberToEdit.FirstName = memberFieldToEdit.FirstName ?? memberToEdit.FirstName;
                memberToEdit.LastName = memberFieldToEdit.LastName ?? memberToEdit.LastName;
                memberToEdit.FullName =
                    MembershipManager.Instance.BuildFullName(memberToEdit.FirstName, memberToEdit.LastName);
                
                _statusCode = HttpStatusCode.OK;
                var updatedMemberString = JsonConvert.SerializeObject(memberToEdit);
                
                ProcessResponse(response, updatedMemberString);
            }
            else
            {
                _statusCode = HttpStatusCode.NotFound;
                ProcessResponse(response, " ");
            }
        }
        public bool IsValid(string resourceName, string[] queryParams)
        {
            return resourceName == "members" && queryParams.Any();
        }
        
        private void ProcessResponse(HttpListenerResponse response, string responseMessage = null)
        {
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseMessage);
            response.ContentType = "application/json";
            response.StatusCode = (int) _statusCode;
            response.ContentLength64 = responseBuffer.Length;
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length); // forces send of response
        }
    }
}