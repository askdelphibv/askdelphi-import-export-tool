using AskDelphi.Tools.EditingAPI;
using AskDelphi.Tools.EditingAPI.EditingAPI;
using AskDelphi.Tools.ImportExport;
using AskDelphi.Tools.ImportExport.ExportTopicsFlow;
using AskDelphi.Tools.ImportExport.ImportTopicsFlow;
using AskDelphi.Tools.ImportExport.Model;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.Tools.TopicImporter
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

                            IEnumerable<Dictionary<string, string>> dataSource = CreateInputDataSource(options, operationContext);

                            var updater = new TopicUpdater(operationContext);
                            int line = 0;
                            foreach (Dictionary<string, string> topicData in dataSource)
                            {
                                line++;
                                Console.WriteLine($"Processing topic ${line}/{dataSource.Count()}");
                                if (!topicData.ContainsKey("topicGuid"))
                                {
                                    Console.Error.WriteLine($"Ignoring entry without a topicGuid field");
                                    continue;
                                }
                                if (!Guid.TryParse(topicData["topicGuid"], out _))
                                {
                                    Console.Error.WriteLine($"Ignoring entry with invalid GUID in the topicGuid field ({topicData["topicGuid"]})");
                                    continue;
                                }

                                await Authenticate(options, operationContext);
                                try
                                {
                                    await updater.Update(new Guid(topicData["topicGuid"]), topicData);
                                }
                                catch (EditingAPI.APIException ex)
                                {
                                    Console.Error.WriteLine($"Update failed for {topicData["topicGuid"]}: API error: {ex.Message}");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Update failed for {topicData["topicGuid"]}: Runtime error: {ex.Message} ({ex.GetType().Name})");
                                }
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

        private static IEnumerable<Dictionary<string, string>> CreateInputDataSource(Options options, OperationContext operationContext)
        {
            string extension = System.IO.Path.GetExtension(options.InputFilename ?? "").ToLowerInvariant();
            switch (extension)
            {
                case ".json":
                    return new JsonInputDataSource(options.InputFilename);
                case ".xlsx":
                    return new ExcelInputDataSource(options.InputFilename);
                default:
                    Console.Error.Write($"Unsupported extension {extension} in '${options.InputFilename}'");
                    Environment.Exit(1);
                    break;
            }
            return null;
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
