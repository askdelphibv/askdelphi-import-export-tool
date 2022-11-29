using AskDelphi.Tools.EditingAPI.EditingAPI;
using AskDelphi.Tools.ImportExport.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.ExportTopicsFlow
{
    public class JsonPathTesterOutputFile : IOutputFile
    {
        private readonly OperationContext operationContext;

        public JsonPathTesterOutputFile(OperationContext operationContext)
        {
            this.operationContext = operationContext;
            Console.WriteLine($"Now only showing the values of mapping the JSON Path '{operationContext.TestJSONPath}', not generating any output files. To show the full document you can match against, specify '-j \"$\"' as a test command. You can then develop your JSON path expression using a site like https://jsonpath.com/ using any single document returned as input.");
        }

        public void AddTopic(TopicTypeEditorDefinition parts)
        {
            JObject source = JObject.FromObject(parts);
            JToken token = source.SelectToken("$.BasicData.TopicGuid");
            string topicGuid = token?.ToObject<string>();

            string output = "<no match>";
            token = source.SelectToken(operationContext.TestJSONPath);
            if (null != token)
            {
                output = token.ToString(Newtonsoft.Json.Formatting.Indented);
            }

            Console.WriteLine($"********** {topicGuid} **********");
            Console.WriteLine(output);
        }

        public void Close()
        {
        }
    }
}
