using Microsoft.Azure.TokenService.Management.Model;
using System.Threading.Tasks;

namespace Microsoft.Azure.TokenService.Management.Interfaces
{
    interface IResponseAzureResource<T> where T : IResponsePropertyBag, new()
    {
        public Task<ResponseProertyBagBase<T>> GetAllAsync();

        public Task<ResponseProertyBagBase<T>> GetAsync(string ResourceName);
    }
}
