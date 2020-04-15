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
    public class GroupResource : AzureResourceBase, IRequestAzureResource<GroupRequestPropertyBag>, IResponseAzureResource<GroupResponsePropertyBag>
    {
        public GroupResource() : base() { }

        public async Task<ResponseProertyBagBase<GroupResponsePropertyBag>> GetAllAsync()
        {
            string responseContent =
            await Connection.HttpGETResponseContentString(new Uri(SDKConstants.ManagementEndPoint + makeResourceURL(string.Empty) + "?api-version=" + SDKConstants.TokenServiceAPIVersion));

            return JsonConvert.DeserializeObject<Model.ResponseProertyBagBase<GroupResponsePropertyBag>>(responseContent);

        }

        public async Task<ResponseProertyBagBase<GroupResponsePropertyBag>> GetAsync(string ResourceName)
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

            var jObjectBlockchainProperty = (JObject)response.Properties;
            var groupProperty = new GroupResponsePropertyBag()
            {
                description = jObjectBlockchainProperty.GetValue("description").ToString()
            };

            var retObject =
              new ResponseResourceObject<GroupResponsePropertyBag>()
              {
                  id = response.Id,
                  location = response.Location,
                  name = response.Name,
                  type = response.Type,
                  properties = groupProperty
              };

            ResponseResourceObject<GroupResponsePropertyBag>[] responseArry = { retObject };

            return new ResponseProertyBagBase<GroupResponsePropertyBag>()
            {
                value = responseArry
            };
        }

        public async Task<GenericResource> RegisterOrUpdateAsync(GroupRequestPropertyBag Properties)
        {
            var resourceURL = makeResourceURL(Properties.GroupName);
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

        private string makeResourceURL(string GroupName)
        {
            return $"/subscriptions/{SDKConstants.AzureSubscriptionId}/resourceGroups/{SDKConstants.ABTResourceGroupName}/providers/{SDKConstants.TokenServiceProviderNamespace}/{SDKConstants.TokenServiceResourceType}/{SDKConstants.ServiceResourceName}/{SDKConstants.PartyResourceType}/{GroupName}";
        }
    }
}
