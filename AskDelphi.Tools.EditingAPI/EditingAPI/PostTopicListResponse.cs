using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    public class PostTopicListResponse
    {
        public PagedResult<TopicListEntryIdentifier> TopicList { get; set; }
    }
}
