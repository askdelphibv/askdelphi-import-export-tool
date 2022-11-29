using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI.EditingAPI
{
    public class PostTopicWorkflowStateRequest
    {
        public TopicWorkflowStateAction Action { get; set; }
    }

    public enum TopicWorkflowStateAction
    {
        CheckIn,
        CheckOut,
        UndoCheckOut
    }
}
