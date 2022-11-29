using AskDelphi.Tools.EditingAPI.EditingAPI.Editors.ImageMap;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class TypeDependentConfiguration
    {
        public ListConfiguration List { get; set; }
        public RichTextEditorConfiguration RichTextEditor { get; set; }
        public SelectOneConfiguration SelectOne { get; set; }
        public SingleTopicChooserConfiguration SingleTopic { get; set; }
        public StringConfiguration String { get; set; }
        public TextConfiguration Text { get; set; }
        public ToggleConfiguration Toggle { get; set; }
        public FileConfiguration File { get; set; }
        /// <summary>
        /// Contains configuration for the tree/hierarchy editor
        /// </summary>
        public TreeConfiguration Tree { get; set; }
        /// <summary>
        /// Contains configuration for the image map editor
        /// </summary>
        public ImageMapConfiguration ImageMap { get; set; }
    }
}