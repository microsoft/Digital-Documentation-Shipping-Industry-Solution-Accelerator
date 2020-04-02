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
    public class AccountResource : AzureResourceBase, IRequestAzureResource<AccountRequestPropertyBag>, IResponseAzureResource<AccountResponsePropertyBag>
    {
        public AccountResource() : base() { }

        private string _groupName;
        public string GroupName
        {
            get
            {
                if (_groupName == null)
                {
                    throw new Exception("GroupName should be assigned");
                }
                else
                {
                    return _groupName;
                }
            }
            set { _groupName = value; }
        }

        public async Task<ResponseProertyBagBase<AccountResponsePropertyBag>> GetAllAsync()
        {
            string accountResourceURL =
                 $"/subscriptions/{SDKConstants.AzureSubscriptionId}/resourceGroups/{SDKConstants.ABTResourceGroupName}/providers/{SDKConstants.TokenServiceProviderNamespace}/{SDKConstants.TokenServiceResourceType}/{SDKConstants.ServiceResourceName}/{SDKConstants.PartyResourceType}/{this.GroupName}/{SDKConstants.AccountResourceType}/";

            string responseContent =
                await Connection.HttpGETResponseContentString(new Uri(SDKConstants.ManagementEndPoint + accountResourceURL + "?api-version=" + SDKConstants.TokenServiceAPIVersion));

            return JsonConvert.DeserializeObject<Model.ResponseProertyBagBase<AccountResponsePropertyBag>>(responseContent);

        }

        public async Task<ResponseProertyBagBase<AccountResponsePropertyBag>> GetAsync(string ResourceName)
        {
            GenericResource response = null;

            try
            {
                response = await ResourceClient.Resources.GetByIdAsync(makeResourceURL(ResourceName),
                            SDKConstants.TokenServiceAPIVersion);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("found")) return null;

                throw e;
            }


            var jObjectAccountProperty = (JObject)response.Properties;
            var accountProperty = new AccountResponsePropertyBag()
            {
                //blockchainNetworkName = jObjectAccountProperty.GetValue("blockchainNetworkName").ToString(),
                description = jObjectAccountProperty.GetValue("description").ToString(),
                provisioningState = jObjectAccountProperty.GetValue("provisioningState").ToString(),
                publicAddress = jObjectAccountProperty.GetValue("publicAddress").ToString()
            };

            var retObject =
             new ResponseResourceObject<AccountResponsePropertyBag>()
             {
                 id = response.Id,
                 location = response.Location,
                 name = response.Name,
                 type = response.Type,
                 properties = accountProperty
             };

            ResponseResourceObject<AccountResponsePropertyBag>[] responseArry = { retObject };

            return new ResponseProertyBagBase<AccountResponsePropertyBag>()
            {
                value = responseArry
            };
        }

        public async Task<GenericResource> RegisterOrUpdateAsync(AccountRequestPropertyBag Properties)
        {
            var resourceURL = makeResourceURL(Properties.AccountName);
            return await ResourceClient.Resources.CreateOrUpdateByIdAsync(resourceURL,
                SDKConstants.TokenServiceAPIVersion, new GenericResource()
                {
                    Location = SDKConstants.ResourceLocation,
                    Properties = Properties.ToDictionary()
                });
        }

        public async Task UnRegisterAsync(string ResourceName)
        {
            await ResourceClient.Resources.DeleteByIdAsync(makeResourceURL(ResourceName),
                                                               SDKConstants.TokenServiceAPIVersion);
        }

        private string makeResourceURL(string AccountName)
        {
            return $"/subscriptions/{SDKConstants.AzureSubscriptionId}/resourceGroups/{SDKConstants.ABTResourceGroupName}/providers/{SDKConstants.TokenServiceProviderNamespace}/{SDKConstants.TokenServiceResourceType}/{SDKConstants.ServiceResourceName}/{SDKConstants.PartyResourceType}/{this.GroupName}/{SDKConstants.AccountResourceType}/{AccountName}";
        }
    }
}
