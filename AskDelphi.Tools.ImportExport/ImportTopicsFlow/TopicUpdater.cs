using AskDelphi.Tools.EditingAPI;
using AskDelphi.Tools.EditingAPI.EditingAPI;
using AskDelphi.Tools.ImportExport.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.ImportTopicsFlow
{
    public class TopicUpdater
    {
        private OperationContext operationContext;
        private TopicAPIWrapper topicApiWrapper;

        public TopicUpdater(OperationContext operationContext)
        {
            this.operationContext = operationContext;
            this.topicApiWrapper = new TopicAPIWrapper(operationContext.EditingApiBaseUrl, operationContext.TenantGuid, operationContext.ProjectGuid, operationContext.ACLGuid);
        }

        public async Task Update(Guid guid, Dictionary<string, string> topicData)
        {
            EditingAPI.EditingAPI.APIResponse<EditingAPI.EditingAPI.GetTopicContentPartsResponse> topicJSON = await topicApiWrapper.GetContentTopicParts(operationContext.EditingAPIToken, guid);
            if (!topicJSON.Success)
            {
                throw new APIException($"Could not load topic with Guid {guid}");
            }
            JObject source = JObject.FromObject(topicJSON.Response.TopicEditorData);

            var updatableFields = topicData.Where(x => x.Key != "topicGuid").ToList();
            bool changed = false;
            foreach (var field in updatableFields)
            {
                MappingEntry mapping = operationContext.Configuration.Mappings.FirstOrDefault(m => m.TargetField == field.Key);
                if (null != mapping)
                {
                    JToken token = source.SelectToken(mapping.JSONPath);
                    if (null == token)
                    {
                        continue; // data is not present
                    }

                    // The path is sth like $.Groups[n].Parts[m].Editors[n].Value.String.Value
                    // We need to persist that part if we change this value.
                    string tokenAsString = TokenAsString(token);
                    if (token.Path.Contains(".Parts[") && !string.Equals(tokenAsString, field.Value))
                    {
                        Console.WriteLine($"Updating '${field.Key}' for topic '{guid}' from '{tokenAsString}' to '{field.Value}'...");

                        string partPath = Regex.Replace(token.Path, @"[.]Editors\[.*$", "");
                        JToken part = source.SelectToken(partPath);
                        JProperty property = token.Parent as JProperty;
                        property.Value = field.Value;

                        // This should be an editor value, so, we need parent^4
                        var partObj = part.ToObject<TopicPart>();

                        await topicApiWrapper.UpdatePartAsync(operationContext.EditingAPIToken, guid, partObj);

                        changed = true;
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Mapping in import file for column '{field.Key}' does not exist in configuration JSON.");
                }
            }
            if (!changed)
            {
                Console.WriteLine($"Found topic '{guid}' in input, but nothing has changed...");
            }
        }

        private static string TokenAsString(JToken token)
        {
            string stringValue;
            if (token.Type == JTokenType.String || token.Type == JTokenType.Guid)
            {
                stringValue = token.ToObject<string>();
            }
            else
            {
                stringValue = token.ToString(Newtonsoft.Json.Formatting.None);
            }

            return stringValue;
        }
    }
}
