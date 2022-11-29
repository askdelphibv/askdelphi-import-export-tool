using AskDelphi.Tools.EditingAPI;
using AskDelphi.Tools.EditingAPI.EditingAPI;
using AskDelphi.Tools.ImportExport;
using AskDelphi.Tools.ImportExport.ExportTopicsFlow;
using AskDelphi.Tools.ImportExport.Model;
using CommandLine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.Tools.TopicExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(
                async () =>
                {
                    await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
                    {
                        try
                        {
                            OperationContext operationContext = new OperationContext();
                            
                            await Authenticate(options, operationContext); // first authentication obtains the URL
                            InitializeCoreGuidsFromOptions(options, operationContext);
                            ReadTopicConfiguration(options, operationContext);

                            TopicList topicList = new TopicList(operationContext, async () =>
                            {
                                await Authenticate(options, operationContext);
                            });
                            await topicList.InitializeFromAPI();

                            CreateOutputFile(options, operationContext);
                            try
                            {
                                int counter = 1;
                                foreach (TopicTypeEditorDefinition topic in topicList)
                                {
                                    operationContext.OutputFile.AddTopic(topic);
                                    Console.WriteLine($"Wrote topic {counter}/{operationContext.TopicCount}...");
                                    Console.Out.Flush();
                                    counter++;
                                }
                            }
                            finally
                            {
                                operationContext.OutputFile.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed with error: {ex.Message} ({ex.GetType().Name})");
                            Environment.Exit(1);
                        }
                    });
                }).Wait();
        }

        private static void CreateOutputFile(Options options, OperationContext operationContext)
        {
            if (!string.IsNullOrWhiteSpace(operationContext.TestJSONPath))
            {
                operationContext.OutputFile = new JsonPathTesterOutputFile(operationContext);
            }
            else if (options.OutputFilename.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            {
                operationContext.OutputFile = new JsonOutputFile(options.OutputFilename, operationContext);
            }
            else if (options.OutputFilename.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
            {
                operationContext.OutputFile = new ExcelOutputFile(options.OutputFilename, operationContext);
            }
            else
            {
                Console.Error.WriteLine($"Unsupported extension: {System.IO.Path.GetExtension(options.OutputFilename)}, we only support .json or .xlsx");
            }
        }

        private static async Task Authenticate(Options options, OperationContext operationContext)
        {
            if (string.IsNullOrWhiteSpace(operationContext.EditingAPIToken) || JwtUtils.GetExpiryTimestamp(operationContext.EditingAPIToken) <= DateTime.UtcNow + TimeSpan.FromMinutes(1))
            {
                var authenticationAPI = new AuthenticationAPI();
                string token = await authenticationAPI.GetEditAPIAuthorizationTokenForSessionCodeAsync(options.SessionCode, options.TenantGuid, options.HostingEnvironmentGuid, options.AuthenticationDataFile);
                operationContext.EditingAPIToken = token;
                operationContext.EditingApiBaseUrl = new Uri(options.EditingAPIBaseUrl);
            }
        }

        private static void InitializeCoreGuidsFromOptions(Options options, OperationContext operationContext)
        {
            operationContext.HostingEnvironmentGuid = options.HostingEnvironmentGuid;
            operationContext.TenantGuid = options.TenantGuid;
            operationContext.ProjectGuid = options.ProjectGuid;
            operationContext.ACLGuid = options.ACLGuid;
            operationContext.TestJSONPath = options.TestJSONPath;

            if (operationContext.HostingEnvironmentGuid == Guid.Empty || operationContext.TenantGuid == Guid.Empty || operationContext.ProjectGuid == Guid.Empty || operationContext.ACLGuid == Guid.Empty)
            {
                Console.Error.WriteLine($"One or more of the specified guids for tenant, hosting environment, project or ACL are not valid (empty)");
                Environment.Exit(1);
            }
        }

        private static void ReadTopicConfiguration(Options options, OperationContext operationContext)
        {
            string configFilePath = options.TopicConfigurationFilename;
            try
            {
                operationContext.Configuration = TopicConfiguration.LoadFromFile(configFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read configuration file {options.TopicConfigurationFilename}: {ex.Message} [{ex.GetType().Name}]");
                Environment.Exit(1);
            }

            if (null == operationContext.Configuration)
            {
                Console.Error.Write($"Could not parse topic configuration from '{configFilePath}'");
                Environment.Exit(1);
            }
        }
    }
}
