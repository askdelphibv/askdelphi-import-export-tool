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
    public class JsonOutputFile : IOutputFile
    {
        private readonly string path;
        private readonly OperationContext operationContext;
        private readonly StreamWriter outputFile;
        private bool isFirstEntry = true;
        private JObject nextObjectToWrite = null; // we delay writing so we can comma-separate the JSON output

        public JsonOutputFile(string path, OperationContext operationContext)
        {
            this.path = path;
            this.operationContext = operationContext;
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                Directory.CreateDirectory(dir);
            }
            outputFile = new StreamWriter(path, false, Encoding.UTF8);
            outputFile.AutoFlush = true;
            outputFile.WriteLine("[");
        }

        public void AddTopic(TopicTypeEditorDefinition parts)
        {
            JObject source = JObject.FromObject(parts);
            JObject target = new JObject();
            bool targetIsEmpty = true;
            foreach (var mapping in operationContext.Configuration.Mappings?.ToList() ?? new List<MappingEntry>())
            {
                AppendLastEntry();
                JToken token = source.SelectToken(mapping.JSONPath);
                if (null != token)
                {
                    target.Add(mapping.TargetField, token);
                    targetIsEmpty = false;
                }
            }

            if (!targetIsEmpty)
            {
                nextObjectToWrite = target;
            }
        }

        private void AppendLastEntry()
        {
            if (null != nextObjectToWrite)
            {
                if (!isFirstEntry)
                {
                    outputFile.WriteLine(",");
                }
                isFirstEntry = false;

                outputFile.Write(nextObjectToWrite.ToString(Newtonsoft.Json.Formatting.Indented));
                nextObjectToWrite = null;
            }
        }

        public void Close()
        {
            AppendLastEntry();
            outputFile.WriteLine("");
            outputFile.WriteLine("]");
        }
    }
}
