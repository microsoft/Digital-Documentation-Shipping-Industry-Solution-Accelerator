using Microsoft.Azure.Management.ResourceManager;

namespace Microsoft.Azure.TokenService.Management
{
    public class AzureResourceBase
    {
        protected ResourceManagementClient ResourceClient;
        public AzureResourceBase()
        {
            ResourceClient = Connection.GetConnection();
        }
    }
}
