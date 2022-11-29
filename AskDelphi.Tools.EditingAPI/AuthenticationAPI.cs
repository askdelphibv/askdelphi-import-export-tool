using AskDelphi.Tools.EditingAPI.PortalAPI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.EditingAPI
{
    public class AuthenticationAPI
    {
        const string PortalAPIServer = "https://portal.askdelphi.com";

        /// <summary>
        /// Uses the portal server and the relevant imola instance to generate an editing API token for the user.
        /// </summary>
        /// <param name="sessionCode"></param>
        /// <param name="tenantGuid"></param>
        /// <param name="hostingEnvironmentGuid"></param>
        /// <returns></returns>
        public async Task<string> GetEditAPIAuthorizationTokenForSessionCodeAsync(string sessionCode, Guid tenantGuid, Guid hostingEnvironmentGuid, string cacheFile)
        {
            RegistrationModel response = await ReadFromCacheAndRefresh(cacheFile);

            if (null == response)
            {
                response = await GetSessionRegistrationFromPortalAsync(sessionCode);
            }

            if (!string.IsNullOrWhiteSpace(cacheFile) && null != response)
            {
                string dir = Path.GetDirectoryName(cacheFile);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllText(cacheFile, System.Text.Json.JsonSerializer.Serialize(response));
            }

            Uri uri = new(response.Url);
            var baseUri = uri.GetLeftPart(System.UriPartial.Authority);

            RestClientOptions imolaClientOptions = new(baseUri)
            {
                ThrowOnAnyError = true,
            };
            RestClient imolaClient = new(imolaClientOptions);
            RestRequest imolaRequest = new("api/token/editingapitoken");
            imolaRequest.AddHeader("Authorization", $"Bearer {response.AccessToken}");
            imolaRequest.AddQueryParameter("tenantGuid", tenantGuid);
            imolaRequest.AddQueryParameter("hostingEnvironmentGuid", hostingEnvironmentGuid);
            imolaRequest.AddQueryParameter("publicationGuid", Guid.Empty); // parameter is not really used anyway
            string token = await imolaClient.GetAsync<string>(imolaRequest);

            return token;
        }

        private async Task<RegistrationModel> ReadFromCacheAndRefresh(string cacheFile)
        {
            RegistrationModel response = null;
            if (File.Exists(cacheFile))
            {
                response = System.Text.Json.JsonSerializer.Deserialize<RegistrationModel>(File.ReadAllText(cacheFile));
                string newToken = await RefreshToken(response.Url, response.AccessToken, response.RefreshToken);
                if (null == newToken)
                {
                    response = null;
                }
                else
                {
                    response.AccessToken = newToken;
                }
            }

            return response;
        }

        public class RefreshResponse
        {
            public string Token { get; set; }
            public string Refresh { get; set; }
        }

        public async Task<string> RefreshToken(string url, string token, string refreshToken)
        {
            try
            {
                Uri uri = new(url);
                var baseUri = uri.GetLeftPart(System.UriPartial.Authority);
                RestClientOptions imolaClientOptions = new(baseUri)
                {
                    ThrowOnAnyError = true,
                };
                RestClient imolaClient = new(imolaClientOptions);
                RestRequest imolaRequest = new("api/token/refresh");
                imolaRequest.AddQueryParameter("token", token);
                imolaRequest.AddQueryParameter("refreshToken", refreshToken);
                RefreshResponse response = await imolaClient.GetAsync<RefreshResponse>(imolaRequest);
                return response.Token;
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Could not refresh the existing token, need to request a new token using a new session code.");
                return null;
            }
        }

        public async Task<RegistrationModel> GetSessionRegistrationFromPortalAsync(string sessionCode)
        {
            RestClientOptions portalClientOptions = new(PortalAPIServer)
            {
                ThrowOnAnyError = true,
            };
            RestClient portalClient = new(portalClientOptions);
            RestRequest portalRequest = new("api/session/registration");
            portalRequest.AddQueryParameter("sessionCode", sessionCode);
            RegistrationModel response = await portalClient.GetAsync<RegistrationModel>(portalRequest);
            return response;
        }

    }
}
