using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Azure.TokenService.Management.Interfaces;
using Microsoft.Azure.TokenService.Management.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.TokenService.Management
{
    public class BlockchainNetwork : AzureResourceBase, IRequestAzureResource<BlockchainNetworkRequestPropertyBag>, IResponseAzureResource<BlockchainNetworkResponsePropertyBag>
    {
        public BlockchainNetwork() : base() { }

        public async Task<GenericResource> RegisterOrUpdateAsync(BlockchainNetworkRequestPropertyBag Properties)
        {
            var resourceURL = makeResourceURL(Properties.BlockchainNetworkId);
            return await ResourceClient.Resources.CreateOrUpdateByIdAsync(resourceURL,
                SDKConstants.TokenServiceAPIVersion, new GenericResource()
                {
                    Location = SDKConstants.ResourceLocation,
                    Properties = Properties.ToDictionary()
                });
        }

        public async Task UnRegisterAsync(string BlockchainName)
        {
            await ResourceClient.Resources.DeleteByIdAsync(makeResourceURL(BlockchainName),
                                                               SDKConstants.TokenServiceAPIVersion);
        }

        public async Task<Model.ResponseProertyBagBase<BlockchainNetworkResponsePropertyBag>> GetAsync(string BlockchainNetworkId)
        {
            GenericResource response = null;

            try
            {
                response = await ResourceClient.Resources.GetByIdAsync(makeResourceURL(BlockchainNetworkId),
                            SDKConstants.TokenServiceAPIVersion);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("found")) return null;

                throw e;
            }


            var jObjectBlockchainProperty = (JObject)response.Properties;

            var blockchainNetworkProperty = new BlockchainNetworkResponsePropertyBag()
            {
                blockchainNode = jObjectBlockchainProperty.GetValue("blockchainNode").ToString(),
                blockchainPlatformName = jObjectBlockchainProperty.GetValue("blockchainPlatformName").ToString(),
                blockchainPlatformType = jObjectBlockchainProperty.GetValue("blockchainPlatformType").ToString(),
                description = jObjectBlockchainProperty.GetValue("description").ToString()
            };

            var retObject =
                new ResponseResourceObject<BlockchainNetworkResponsePropertyBag>()
                {
                    id = response.Id,
                    location = response.Location,
                    name = response.Name,
                    type = response.Type,
                    properties = blockchainNetworkProperty
                };

            ResponseResourceObject<BlockchainNetworkResponsePropertyBag>[] responseArry = { retObject };

            return new ResponseProertyBagBase<BlockchainNetworkResponsePropertyBag>()
            {
                value = responseArry
            };
        }

        public async Task<Model.ResponseProertyBagBase<BlockchainNetworkResponsePropertyBag>> GetAllAsync()
        {

            string responseContent =
                await Connection.HttpGETResponseContentString(new Uri(SDKConstants.ManagementEndPoint + makeResourceURL(string.Empty) + "?api-version=" + SDKConstants.TokenServiceAPIVersion));

            return JsonConvert.DeserializeObject<Model.ResponseProertyBagBase<BlockchainNetworkResponsePropertyBag>>(responseContent);

            //"https://api-dogfood.resources.windows-int.net/subscriptions/1e5f5d29-1b9b-4330-bacb-e6a00e4e8a66/resourceGroups/ABTTest/providers/Microsoft.BlockchainTokens/tokenServices/echopreview/blockchainNetworks?api-version=2019-07-19-preview"
            //""https://api-dogfood.resources.windows-int.net//subscriptions/1e5f5d29-1b9b-4330-bacb-e6a00e4e8a66/resourceGroups/ABTTest/providers/Microsoft.BlockchainTokens/tokenServices/echopreview/groups/msft/accounts?api-version=2019-07-19-preview""
            //"https://api-dogfood.resources.windows-int.net/subscriptions/1e5f5d29-1b9b-4330-bacb-e6a00e4e8a66/resourceGroups/ABTTest/providers/Microsoft.BlockchainTokens/tokenServices/echopreview/groups?api-version=2019-07-19-preview"
        }


        private string makeResourceURL(string BlockchainNetowkrName)
        {
            return $"/subscriptions/{SDKConstants.AzureSubscriptionId}/resourceGroups/{SDKConstants.ABTResourceGroupName}/providers/{SDKConstants.TokenServiceProviderNamespace}/{SDKConstants.TokenServiceResourceType}/{SDKConstants.ServiceResourceName}/{SDKConstants.BlockchainNetworkResourceType}/{BlockchainNetowkrName}";
        }


    }
}
