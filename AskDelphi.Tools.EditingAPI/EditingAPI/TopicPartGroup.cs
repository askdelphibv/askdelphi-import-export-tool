using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    public class TopicPartGroup
    {
        public string DefaultLabel { get; set; }
        public string DescriptionHTML { get; set; }
        public string Icon { get; set; }
        public string[] Keywords { get; set; }
        public string PartGroupId { get; set; }
        public TopicPart[] Parts { get; set; }
    }
}
