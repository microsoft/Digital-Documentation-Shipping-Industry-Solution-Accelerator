using Microsoft.Azure.TokenService.Management.Interfaces;

namespace Microsoft.Azure.TokenService.Management.Model
{
    public class GroupResponsePropertyBag : IResponsePropertyBag
    {
        public string description { get; set; }
    }
}
