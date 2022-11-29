using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    public class TopicTypeEditorDefinition
    {
        /// <summary>
        /// The topic guid
        /// </summary>
        public TopicListEntryIdentifier BasicData { get; set; }
        /// <summary>
        /// Grouped data
        /// </summary>
        public TopicPartGroup[] Groups { get; set; }
        /// <summary>
        /// Topic namespace
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Name of the topic type
        /// </summary>
        public string TopicTypeName { get; set; }
    }
}
