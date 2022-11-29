using System.Collections.Generic;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class SingleTopicChooserConfiguration 
    {
        public IEnumerable<string> Namespaces { get; set; }

        /// <summary>
        /// Either "Media" or "Topic"
        /// </summary>
        public string Type { get; set; }
    }
}