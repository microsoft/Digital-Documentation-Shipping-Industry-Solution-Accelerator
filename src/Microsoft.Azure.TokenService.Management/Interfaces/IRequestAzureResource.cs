using Microsoft.Azure.Management.ResourceManager.Models;
using System.Threading.Tasks;

namespace Microsoft.Azure.TokenService.Management.Interfaces
{
    public interface IRequestAzureResource<T> where T : IRequestPropertyBag, new()
    {
        public Task<GenericResource> RegisterOrUpdateAsync(T Properties);
        public Task UnRegisterAsync(string ResourceName);



    }
}
