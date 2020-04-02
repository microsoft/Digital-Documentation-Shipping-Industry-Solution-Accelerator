namespace Microsoft.Azure.TokenService.Management
{
    //public enum UserPersona { RPUser, MicrosoftUser1, MicrosoftUser2, MicrosoftUser3, MicrosoftUser4, BlockchainUser1, BlockchainUser2 }

    public static class SDKConstants
    {
        public const string ClientSecret = "zJWmadWm2Xu9Bexsd9KQCQIAR6Oz.:@]";
        public const string ClientId = "fe898dda-c1b7-408a-b75a-e6776225c6bd";
        public const string AzureTenantId = "f686d426-8d16-42db-81b7-ab578e110ccd";
        public const string AzureSubscriptionId = "1e5f5d29-1b9b-4330-bacb-e6a00e4e8a66";

        //public const string ApiVersion1 = "api/v1/";
        //public const string AccountsUri = ApiVersion1 + "accounts";
        //public const string TokenServicesUri = ApiVersion1 + "tokenservices";
        //public const string BlockchainNetworksUri = ApiVersion1 + "blockchainnetworks";
        //public const string PartysUri = ApiVersion1 + "parties";
        //public const string TemplateUri = ApiVersion1 + "templates";
        //public const string Party1Name = "Party1";


        // Azure PPE constants
        public static string ActiveDirectoryEndpoint = "https://login.windows-ppe.net/";
        public static string ActiveDirectoryServiceEndpointResourceId = "https://management.core.windows.net/";
        public static string ManagementEndPoint = "https://api-dogfood.resources.windows-int.net/";

        // Munich constants
        public static string TokenServiceProviderNamespace = "Microsoft.BlockchainTokens";
        public static string TokenServiceResourceType = "tokenServices";
        public static string BlockchainNetworkResourceType = "blockchainNetworks";
        public static string PartyResourceType = "groups";
        public static string AccountResourceType = "accounts";
        public static string TokenTemplateResourceType = "tokenTemplates";
        public static string TokenServiceAPIVersion = "2019-07-19-preview";
        public static string ABTResourceGroupName = "ABTTest";
        public static string ResourceLocation = "West Central US";
        public static string ServiceResourceName = "echopreview";
    }
}
