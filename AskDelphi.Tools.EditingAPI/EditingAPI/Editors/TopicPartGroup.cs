using System.Collections.Generic;

namespace AskDelphi.Tools.EditingAPI.EditingAPI.Editors
{
    public class TopicPartGroup
    {
        public string DefaultLabel { get; set; }
        public string DescriptionHTML { get; set; }
        public string Icon { get; set;}
        public string[] Keywords { get; set; }
        public string PartGroupId { get; set; }
        public TopicPart[] Parts { get; set; }
    }
}