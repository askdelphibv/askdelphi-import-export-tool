using AskDelphi.Tools.EditingAPI.EditingAPI.Editors.ImageMap;
using System.Collections.Generic;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class EditorValue
    {
        public string EditorFieldId { get; set; }
        public ListValueData List { get; set; }
        public RichTextValueData RichTextEditor { get; set; }
        public SelectOneValueData SelectOne { get; set; }
        public SingleTopicChooserValueData SingleTopic { get; set; }
        public StringValueData String { get; set; }
        public TextValueData Text { get; set; }
        public ToggleValueData Toggle { get; set; }
        public FileValueData File { get; set; }
        /// <summary>
        /// Value and thus data for an image map editor
        /// </summary>
        public ImageMapValueData ImageMap { get; set; }
        /// <summary>
        /// Collection of tree value data used primarily for hierarchy editing
        /// </summary>
        public List<TreeValueData> Tree { get; set; }
    }
}