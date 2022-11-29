using AskDelphi.Tools.EditingAPI.EditingAPI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI
{
    public class TopicAPIWrapper
    {
        private readonly Uri apiUrl;
        private readonly Guid tenantGuid;
        private readonly Guid projectGuid;
        private readonly Guid aclGuid;

        public TopicAPIWrapper(Uri apiUrl, Guid tenantGuid, Guid projectGuid, Guid aclGuid)
        {
            this.apiUrl = apiUrl;
            this.tenantGuid = tenantGuid;
            this.projectGuid = projectGuid;
            this.aclGuid = aclGuid;
        }

        public async Task<APIResponse<GetTopicContentPartsResponse>> GetContentTopicParts(string accessToken, Guid topicId)
        {
            // v1/tenant/{tenantId}/project/{projectId}/acl/{aclEntryId}/topic/{topicId}/publication/{publicationId}/part - get all parts
            // publicationId can be GUID.EMPTY: it will just cause the embed markers to be incomplete (do not care).

            RestClientOptions options = new(apiUrl)
            {
                ThrowOnAnyError = false,
            };
            RestClient client = new RestClient(options);
            RestRequest request = new($"v1/tenant/{tenantGuid}/project/{projectGuid}/acl/{aclGuid}/topic/{topicId}/publication/{Guid.Empty}/part");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.OnBeforeDeserialization = (x) =>
            {
                ApiDebugUtils.DebugAPIResponse(client, request, x);
            };
            APIResponse<GetTopicContentPartsResponse> response = await client.GetAsync<APIResponse<GetTopicContentPartsResponse>>(request);
            return response;
        }

        public async Task<APIResponse<PutContentTopicPartResponse>> UpdatePartAsync(string accessToken, Guid guid, TopicPart partObj)
        {
            await CheckOut(accessToken, guid);

            try
            {
                System.Diagnostics.Trace.WriteLine($"UPDATING: {guid} / {partObj.PartId} TO {System.Text.Json.JsonSerializer.Serialize(partObj)}...");

                RestClientOptions options = new(apiUrl)
                {
                    ThrowOnAnyError = false,
                };
                RestClient client = new RestClient(options);
                RestRequest request = new($"v1/tenant/{tenantGuid}/project/{projectGuid}/acl/{aclGuid}/topic/{guid}/part/{partObj.PartId}");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.OnBeforeDeserialization = (x) =>
                {
                    ApiDebugUtils.DebugAPIResponse(client, request, x);
                };
                request.AddJsonBody(new PutContentTopicPartRequest
                {
                    Part = partObj
                });

                APIResponse<PutContentTopicPartResponse> response = await client.PutAsync<APIResponse<PutContentTopicPartResponse>>(request);
                return response;
            }
            finally
            {
                await CheckIn(accessToken, guid);
            }

        }

        private async Task CheckOut(string editingAPIToken, Guid guid)
        {
            try
            {
                var result = await SetLockedState(editingAPIToken, guid, true);
                if (!result.Response.Status)
                {
                    Console.Error.WriteLine($"API: Could not lock topic {guid}...");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"API: Could not lock topic {guid} with error {ex.GetType().Name} {ex.Message}...");
                throw;
            }
        }

        private async Task CheckIn(string editingAPIToken, Guid guid)
        {
            try
            {
                var result = await SetLockedState(editingAPIToken, guid, false);
                if (!result.Response.Status)
                {
                    Console.Error.WriteLine($"API: Could not lock topic {guid}...");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"API: Could not lock topic {guid} with error {ex.GetType().Name} {ex.Message}...");
                throw;
            }
        }

        public async Task<APIResponse<PostTopicWorkflowReponse>> SetLockedState(string accessToken, Guid guid, bool lockedState)
        {
            RestClientOptions options = new(apiUrl)
            {
                ThrowOnAnyError = false,
            };
            RestClient client = new RestClient(options);
            RestRequest request = new($"v1/tenant/{tenantGuid}/project/{projectGuid}/acl/{aclGuid}/topic/{guid}/workflowstate");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.OnBeforeDeserialization = (x) =>
            {
                ApiDebugUtils.DebugAPIResponse(client, request, x);
            };
            request.AddJsonBody(new PostTopicWorkflowStateRequest
            {
                Action = lockedState ? TopicWorkflowStateAction.CheckOut : TopicWorkflowStateAction.CheckIn
            });

            APIResponse<PostTopicWorkflowReponse> response = await client.PostAsync<APIResponse<PostTopicWorkflowReponse>>(request);
            return response;
        }
    }
}
