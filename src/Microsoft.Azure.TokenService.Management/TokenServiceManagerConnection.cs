using Microsoft.Azure.Management.ResourceManager;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Azure.TokenService.Management
{
    public class Connection
    {
        public static ResourceManagementClient GetConnection()
        {
            var tokenCredentials = new TokenCredentials(Connection.GetAuthorizationToken());

            return new ResourceManagementClient(tokenCredentials)
            {
                SubscriptionId = SDKConstants.AzureSubscriptionId,
                BaseUri = new Uri(SDKConstants.ManagementEndPoint),
            };
        }

        public static string GetAuthorizationToken()
        {
            ClientCredential clientCredential =
                new ClientCredential(SDKConstants.ClientId,
                SDKConstants.ClientSecret);

            var context =
                new AuthenticationContext(
                SDKConstants.ActiveDirectoryEndpoint + SDKConstants.AzureTenantId);

            var result =
                context.AcquireTokenAsync(SDKConstants.ActiveDirectoryServiceEndpointResourceId, clientCredential).GetAwaiter().GetResult();


            if (result == null)
            {
                throw new InvalidOperationException("Fail to get JWT Token");
            }

            return result.AccessToken;
        }

        public static async Task<string> HttpGETResponseContentString(Uri ResourceLocation)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Connection.GetAuthorizationToken());

            httpRequest.Method = HttpMethod.Get;
            httpRequest.RequestUri = ResourceLocation;

            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

            return await response.Content.ReadAsStringAsync();
        }

        #region RestClient Creation for Azure Resource Manage Fluent. (obsolute)
        //private static RestClient CreateRestClient()
        //{
        //    var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
        //        SDKConstants.ClientId,
        //        SDKConstants.ClientSecret,
        //        SDKConstants.AzureTenantId,
        //        new AzureEnvironment()
        //        {
        //            AuthenticationEndpoint = SDKConstants.ActiveDirectoryEndpoint,
        //            ResourceManagerEndpoint = SDKConstants.ManagementEndPoint,
        //            ManagementEndpoint = SDKConstants.ActiveDirectoryServiceEndpointResourceId,
        //            GraphEndpoint = "https://graph.windows-int.net/",
        //            StorageEndpointSuffix = "http://core.windows-int.net/",
        //            KeyVaultSuffix = "vault.azure.net"
        //        });

        //    return RestClient
        //         .Configure()
        //         .WithEnvironment(new AzureEnvironment()
        //         {
        //             AuthenticationEndpoint = SDKConstants.ActiveDirectoryEndpoint,
        //             ResourceManagerEndpoint = SDKConstants.ManagementEndPoint,
        //             ManagementEndpoint = SDKConstants.ActiveDirectoryServiceEndpointResourceId,
        //             GraphEndpoint = "https://graph.windows-int.net/",
        //             StorageEndpointSuffix = "http://core.windows-int.net/",
        //             KeyVaultSuffix = "vault.azure.net"
        //         })
        //         .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
        //         .WithCredentials(credentials)
        //         .WithBaseUri(SDKConstants.ManagementEndPoint)
        //         .Build();
        //}
        #endregion
    }
}
