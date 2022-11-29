using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    /// <summary>
    /// Value data for tree editor/hierarchy editor
    /// </summary>
    public class TreeValueData
    {
        /// <summary>
        /// Unique id of the tree node
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// Title of the node
        /// </summary>
        public string NodeTitle { get; set; }
        /// <summary>
        /// Collection of topics, which are tagged to this node
        /// </summary>
        public List<ResponseTopicTaggedToHierarchy> TaggedTopicIds { get; set; }
        /// <summary>
        /// Value data for more information topic
        /// </summary>
        public SingleTopicChooserValueData MoreInformationTopic { get; set; }
        /// <summary>
        /// Child node collection
        /// </summary>
        public List<TreeValueData> Children { get; set; }
    }
}
