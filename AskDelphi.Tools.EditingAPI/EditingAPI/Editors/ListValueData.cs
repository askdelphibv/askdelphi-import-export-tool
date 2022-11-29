using System.Collections.Generic;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class ListValueData
    {
        /// <summary>
        /// Optional property, indicates the unique id of the list item
        /// </summary>
        public string Id { get; set; }
        public EditorValue[] Values { get; set; }
    }
}