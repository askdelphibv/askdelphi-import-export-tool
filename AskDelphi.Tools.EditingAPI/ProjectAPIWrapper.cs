using AskDelphi.Tools.EditingAPI.EditingAPI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI
{
    public class ProjectAPIWrapper
    {
        private readonly Uri apiUrl;
        private readonly Guid tenantGuid;
        private readonly Guid projectGuid;
        private readonly Guid aclGuid;

        public ProjectAPIWrapper(Uri apiUrl, Guid tenantGuid, Guid projectGuid, Guid aclGuid)
        {
            this.apiUrl = apiUrl;
            this.tenantGuid = tenantGuid;
            this.projectGuid = projectGuid;
            this.aclGuid = aclGuid;
        }

        public async Task<APIResponse<PostTopicListResponse>> LoadTopicList(string accessToken, PostTopicListRequest topicListRequest)
        {
            // POST v1/tenant/{tenantId}/project/{projectId}/acl/{aclEntryId}/topiclist
            //      - request: { 
            //          - public Guid[] TopicTypes { get; set; }
            //          - public string[] Namespaces { get; set; }
            //
            try
            {
                RestClientOptions options = new(apiUrl)
                {
                    ThrowOnAnyError = false,

                };
                RestClient client = new RestClient(options);
                RestRequest request = new($"v1/tenant/{tenantGuid}/project/{projectGuid}/acl/{aclGuid}/topiclist");
                request.RequestFormat = DataFormat.Json;
                request.OnBeforeDeserialization = (x) =>
                {
                    ApiDebugUtils.DebugAPIResponse(client, request, x);
                };
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AddJsonBody(topicListRequest);

                APIResponse<PostTopicListResponse> response = await client.PostAsync<APIResponse<PostTopicListResponse>>(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new APIException($"API call failed with unknown error", ex);
            }
        }
    }
}
