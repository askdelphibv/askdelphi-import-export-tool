using AskDelphi.Tools.EditingAPI;
using AskDelphi.Tools.EditingAPI.EditingAPI;
using AskDelphi.Tools.ImportExport.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.ExportTopicsFlow
{
    public class TopicList : IEnumerable<TopicTypeEditorDefinition>
    {
        private readonly OperationContext operationContext;
        private readonly Func<Task> authenticate;
        private readonly ProjectAPIWrapper projectAPI;

        public TopicList(OperationContext operationContext, Func<Task> authenticate)
        {
            this.operationContext = operationContext;
            this.authenticate = authenticate;
            projectAPI = new ProjectAPIWrapper(operationContext.EditingApiBaseUrl, operationContext.TenantGuid, operationContext.ProjectGuid, operationContext.ACLGuid);
        }

        public async Task InitializeFromAPI()
        {
            PostTopicListRequest request = InitializePostTopicListRequest();
            request.page = 1;
            request.pageSize = 1;
            request.onlyEditable = false;
            request.orderBy = "title";

            this.authenticate?.Invoke()?.Wait();
            var result = await projectAPI.LoadTopicList(operationContext.EditingAPIToken, request);
            if (!result.Success)
            {
                throw new APIException($"Could not load topic list", result.ErrorCode, result.ErrorMessage);
            }

            operationContext.TopicCount = result.Response.TopicList.TotalAvailable;
        }

        private PostTopicListRequest InitializePostTopicListRequest()
        {
            PostTopicListRequest request = new PostTopicListRequest();
            request.namespaces = new string[] { operationContext.Configuration.Namespace };
            if (operationContext.Configuration.TopicTypes != null && operationContext.Configuration.TopicTypes.Length > 0)
            {
                request.topicTypes = operationContext.Configuration.TopicTypes.ToArray();
            }
            return request;
        }

        public IEnumerator<TopicTypeEditorDefinition> GetEnumerator()
        {
            return new TopicListEnumerator(operationContext, authenticate);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TopicListEnumerator(operationContext, authenticate);
        }

        private class TopicListEnumerator : IEnumerator<TopicTypeEditorDefinition>
        {
   
            private readonly OperationContext operationContext;
            private readonly Func<Task> authenticate;
            private readonly ProjectAPIWrapper projectAPI;
            private readonly TopicAPIWrapper topicAPI;
            private int currentIndex = -1;
            private const int pageSize = 10;
            private Dictionary<int, PostTopicListResponse> responses = new Dictionary<int, PostTopicListResponse>();

            public TopicListEnumerator(OperationContext operationContext, Func<Task> authenticate)
            {
                this.operationContext = operationContext;
                this.authenticate = authenticate;
                projectAPI = new ProjectAPIWrapper(operationContext.EditingApiBaseUrl, operationContext.TenantGuid, operationContext.ProjectGuid, operationContext.ACLGuid);
                topicAPI = new TopicAPIWrapper(operationContext.EditingApiBaseUrl, operationContext.TenantGuid, operationContext.ProjectGuid, operationContext.ACLGuid);
            }

            public TopicTypeEditorDefinition Current
            {
                get
                {
                    LoadCurrentPage();

                    List<TopicListEntryIdentifier> currentPage = responses[currentIndex / pageSize].TopicList.Result.ToList();
                    TopicListEntryIdentifier topicListEntry = currentPage[currentIndex % pageSize];
                    Task<APIResponse<GetTopicContentPartsResponse>> task = LoadTopicContent(topicListEntry);
                    return task.Result.Response.TopicEditorData;
                }
            }

            private Task<APIResponse<GetTopicContentPartsResponse>> LoadTopicContent(TopicListEntryIdentifier topicListEntry)
            {
                this.authenticate?.Invoke()?.Wait();
                Task<APIResponse<GetTopicContentPartsResponse>> task = topicAPI.GetContentTopicParts(operationContext.EditingAPIToken, topicListEntry.TopicGuid);
                if (!task.Result.Success)
                {
                    throw new APIException($"Could not load topic at index {currentIndex} for guid {topicListEntry.TopicGuid}", task.Result.ErrorCode, task.Result.ErrorMessage);
                }
                task.Result.Response.TopicEditorData.BasicData = topicListEntry;

                return task;
            }

            private void LoadCurrentPage()
            {
                if (!responses.ContainsKey(currentIndex / pageSize))
                {
                    var request = InitializePostTopicListRequest();
                    request.page = 1 + (currentIndex / pageSize);

                    Task<APIResponse<PostTopicListResponse>> task = projectAPI.LoadTopicList(operationContext.EditingAPIToken, request);
                    task.Wait();
                    if (!task.Result.Success)
                    {
                        throw new APIException($"Could not load topic list page for index {currentIndex}", task.Result.ErrorCode, task.Result.ErrorMessage);
                    }
                    responses[currentIndex / pageSize] = task.Result.Response;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < operationContext.TopicCount;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            private PostTopicListRequest InitializePostTopicListRequest()
            {
                PostTopicListRequest request = new PostTopicListRequest();
                request.namespaces = new string[] { operationContext.Configuration.Namespace };
                if (operationContext.Configuration.TopicTypes != null && operationContext.Configuration.TopicTypes.Length > 0)
                {
                    request.topicTypes = operationContext.Configuration.TopicTypes.ToArray();
                }
                request.page = 1;
                request.pageSize = pageSize;
                request.onlyEditable = false;
                request.orderBy = "title";
                return request;
            }

        }
    }
}
