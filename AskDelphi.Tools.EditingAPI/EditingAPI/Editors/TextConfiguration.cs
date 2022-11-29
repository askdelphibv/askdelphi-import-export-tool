namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class TextConfiguration 
    {
        public int MaxLength { get; set; }
        public int PreferredNumberOfLines { get; set; }
        /// <summary>
        /// Indicates whether this editor supports preview
        /// </summary>
        public bool SupportsPreview { get; set; }
    }
}