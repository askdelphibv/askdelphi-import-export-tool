using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class ResponseTopicTaggedToHierarchy
    {
        /// <summary>
        /// The id of the node to which the topic is tagged
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// The topic guid
        /// </summary>
        public Guid TopicGuid { get; set; }
        /// <summary>
        /// The namespace of the topic
        /// </summary>
        public string TopicNamespace { get; set; }
        /// <summary>
        /// The title of the topic
        /// </summary>
        public string TopicTitle { get; set; }
    }
}
