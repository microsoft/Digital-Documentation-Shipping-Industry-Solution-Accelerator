using System.Collections.Generic;

namespace Microsoft.Azure.TokenService.Management.Interfaces
{
    public interface IRequestPropertyBag
    {
        public Dictionary<string, object> ToDictionary();
    }
}
