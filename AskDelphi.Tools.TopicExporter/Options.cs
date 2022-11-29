using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.TopicExporter
{
    class Options
    {
        [Option('s', "session-code", Required = false, 
            HelpText = "Single-use session code. You can obtain this using the mobile QR code feature in the publication. These codes are single-use so a new code is required for every run.")]
        public string SessionCode { get; set; }

        [Option('t', "tenantid", Required = true, 
            HelpText = "The tenant guid for the tenant in whose context the operation is carried out. This can be obtained from the hosting environment configuration editor, or from AskDelphi support.")]
        public Guid TenantGuid { get; set; }

        [Option('h', "hostid", Required = true, 
            HelpText = "The hosting environment guid for the tenant in whose context the operation is carried out. This can be obtained from the hosting environment configuration editor, or from AskDelphi support.")]
        public Guid HostingEnvironmentGuid { get; set; }

        [Option('a', "aclid", Required = true, 
            HelpText = "Identifier of the ACL that grants editing access to the user for whom the session code was created. You can get this from the ACL editor for the AskDelphi project.")]
        public Guid ACLGuid { get; set; }

        [Option('p', "projectid", Required = true, 
            HelpText = "Identifies the project for which the tool needs to be run, copy this from the project properties view.")]
        public Guid ProjectGuid { get; set; }

        [Option('c', "config", Required = true, 
            HelpText = "Specially crafted mapping file that describes which information from the topic should be written into the output where.")]
        public string TopicConfigurationFilename { get; set; }

        [Option('o', "out", Required = true, 
            HelpText = "Output filename.")]
        public string OutputFilename { get; set; }

        [Option('x', "api", Required = false, 
            HelpText = "Full URL to the editing API server that is to be used, defaults to the production server, specify only when testing with other environments such as staging or test.", 
            Default = "https://askdelphi-prod-editing-api.azurewebsites.net/")]
        public string EditingAPIBaseUrl { get; set; }

        [Option('b', "save-auth-file", Required = false, Default = null,
            HelpText = "Authentication data file name. If specified login information is stored here between sessions and is re-used.")]
        public string AuthenticationDataFile { get; set; }

        [Option('j', "test-jsonpath", Required = false, Default = null,
            HelpText = "If specified, writes the result of the JSON path specified as argument to this option to the console for each matching entry, does not generate any output file.")]
        public string TestJSONPath { get; set; }
    }
}
