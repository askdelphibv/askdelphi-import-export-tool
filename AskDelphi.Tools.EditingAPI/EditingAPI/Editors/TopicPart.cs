using System.Collections.Generic;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class TopicPart
    {
        public string DefaultLabel { get; set; }
        public string DescriptionHTML { get; set; }
        public TopicPartFieldEditor[] Editors { get; set; }
        public string[] Keywords { get; set; }
        public string PartId { get; set; }
    }
}