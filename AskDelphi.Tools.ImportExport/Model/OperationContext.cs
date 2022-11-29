using AskDelphi.Tools.ImportExport.ExportTopicsFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.Model
{
    public class OperationContext
    {
        public string EditingAPIToken { get; set; }
        public Uri EditingApiBaseUrl { get; set; }

        public Guid TenantGuid { get; set; }
        public Guid HostingEnvironmentGuid { get; set; }
        public Guid ProjectGuid { get; set; }
        public Guid ACLGuid { get; set; }

        public TopicConfiguration Configuration { get; set; }
        public IOutputFile OutputFile { get; set; }

        /// <summary>
        /// State: Total number of matching topics.
        /// </summary>
        public int TopicCount { get; set; }

        /// <summary>
        /// Only relevant for export: JSONPath expression to be used.
        /// </summary>
        public string TestJSONPath { get; set; }
    }
}
