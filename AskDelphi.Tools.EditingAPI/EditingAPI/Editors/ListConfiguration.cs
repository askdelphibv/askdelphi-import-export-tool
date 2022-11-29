using System.Collections.Generic;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class ListConfiguration 
    {
        /// <summary>
        /// Configuration of items that make up the list
        /// </summary>
        public TopicPart ItemConfiguration { get; set; }
        /// <summary>
        /// Editor is used as title to display for example in an accordion header
        /// </summary>
        public string EditorIdUsedAsTitle { get; set; }

        /// <summary>
        /// True if the user should eb able to add or delete data to/from the list.
        /// </summary>
        public bool SupportAddDelete { get; set; }

        /// <summary>
        /// True if the individual list items should be collapsible.
        /// </summary>
        public bool SupportCollapse { get; set; }

        /// <summary>
        /// True if the user should be abblowed to re-order the list
        /// </summary>
        public bool SupportOrdering { get; set; }
    }
}