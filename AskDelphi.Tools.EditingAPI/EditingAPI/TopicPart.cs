using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
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
