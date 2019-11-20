using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Configuration.ConfigurationBuilders;
using Microsoft.Extensions.Configuration;

namespace MembershipApp.WebApi
{
    public class FrameworklessApi
    {
        private readonly ApiGetResponseFactory _apiGetResponseFactory;
        private readonly ApiDeleteResponseFactory _apiDeleteResponseFactory;
        private readonly ApiPostResponseFactory _apiPostResponseFactory;
        private readonly ApiPatchResponseFactory _apiPatchResponseFactory;
        private HttpListenerRequest _request;
        private HttpListenerResponse _response;

        public FrameworklessApi(ApiGetResponseFactory apiGetResponseFactory,
            ApiDeleteResponseFactory apiDeleteResponseFactory, ApiPostResponseFactory apiPostResponseFactory,
            ApiPatchResponseFactory apiPatchResponseFactory)
        {
            _apiGetResponseFactory = apiGetResponseFactory;
            _apiDeleteResponseFactory = apiDeleteResponseFactory;
            _apiPostResponseFactory = apiPostResponseFactory;
            _apiPatchResponseFactory = apiPatchResponseFactory;
        }

        public void Listen()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var url = config["AppSettings:prod"];

            var server = new HttpListener();
            server.Prefixes.Add(url);
            server.Start();
            Console.WriteLine("Listening....");

            while (true)
            {
                var context = server.GetContext(); // Gets the request
                _request = context.Request;
                _response = context.Response;

                var apiRequestedResourceName = ExtractResourceNameInRequestedUrl();
                var queryParams = ExtractQueryParamsFromRequestedUrl();

                Console.WriteLine($"{_request.HttpMethod} request made to {context.Request.Url}");

                ProcessHttpMethod(apiRequestedResourceName, queryParams);
            }
        }

        private void ProcessHttpMethod(string apiRequestedResourceName, string[] queryParams)
        {
            if (_request.HttpMethod == "GET")
            {
                var getResponse =
                    _apiGetResponseFactory.CreateGetResponseObject(apiRequestedResourceName, queryParams);

                if (queryParams.Length > 0)
                {
                    getResponse.ProcessRequest(_response, queryParams[0]);
                }
                else
                {
                    getResponse.ProcessRequest(_response);
                }
            }

            if (_request.HttpMethod == "DELETE")
            {
                if (queryParams.Any())
                {
                    var deleteResponse =
                        _apiDeleteResponseFactory.CreateDeleteResponseObject(apiRequestedResourceName, queryParams[0]);
                    deleteResponse.ProcessRequest(_response, queryParams[0]);
                }
                else
                {
                    var deleteResponse =
                        _apiDeleteResponseFactory.CreateDeleteResponseObject(apiRequestedResourceName, string.Empty);
                    deleteResponse.ProcessRequest(_response, string.Empty);
                }
            }

            if (_request.HttpMethod == "POST")
            {
                var postAction =
                    _apiPostResponseFactory.CreatePostResponseObject(apiRequestedResourceName, queryParams);

                postAction.ProcessRequest(_request, _response);
            }

            if (_request.HttpMethod == "PATCH")
            {
                var patchRequest =
                    _apiPatchResponseFactory.CreatePatchResponseObject(apiRequestedResourceName, queryParams);

                patchRequest.ProcessRequest(_request, _response, queryParams[0]);
            }
        }

        private string[] ExtractQueryParamsFromRequestedUrl()
        {
            return _request.Url.Segments.Skip(2).Select(s => s.Replace("/", "")).ToArray();
        }

        private string ExtractResourceNameInRequestedUrl()
        {
            return _request.Url.Segments[1].Replace("/", "");
        }
    }
}