using AskDelphi.Tools.EditingAPI.EditingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport
{
    public interface IOutputFile
    {
        void AddTopic(TopicTypeEditorDefinition parts);

        void Close();
    }
}
