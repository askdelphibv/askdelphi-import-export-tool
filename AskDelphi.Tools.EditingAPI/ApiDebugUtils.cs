using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI
{
    public static class ApiDebugUtils
    {
        internal static void DebugAPIResponse(RestClient client, RestRequest request, RestResponse x)
        {
            System.Diagnostics.Trace.WriteLine($"{request.Method} {client.BuildUri(request)} => {x.Content}");
        }
    }
}
