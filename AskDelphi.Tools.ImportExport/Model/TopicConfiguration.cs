using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.Model
{
    public class TopicConfiguration
    {
        public string Namespace { get; set; }
        public Guid[] TopicTypes { get; set; }
        public IEnumerable<MappingEntry> Mappings { get; set; }

        public static TopicConfiguration LoadFromFile(string configFilePath)
        {
            TopicConfiguration topicConfiguration;
            string configurationFile = System.IO.File.ReadAllText(configFilePath);
            topicConfiguration = System.Text.Json.JsonSerializer.Deserialize<TopicConfiguration>(configurationFile);
            if (null != topicConfiguration)
            {
                var mappingList = topicConfiguration.Mappings?.ToList() ?? new System.Collections.Generic.List<MappingEntry>();
                mappingList.Insert(0, new MappingEntry { JSONPath = "$.BasicData.TopicGuid", TargetField = "topicGuid" }); // always include this mapping
                topicConfiguration.Mappings = mappingList;
            }

            return topicConfiguration;
        }
    }
}
