using Microsoft.Azure.TokenService;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.IO;
using System.Reflection;

namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class TokenAPIService : ITokenService
    {
        public AzureTokenServiceAPI Initialize()
        {

            //var clientCreds = await ApplicationTokenProvider.LoginSilentAsync(
            //                "f686d426-8d16-42db-81b7-ab578e110ccd",
            //                "aeae2788-6f1b-4e2b-bae9-45e7fe76a723",
            //                "7y=ko=AZM@.JgBdfi2C7i2k8q3o=TtkC",
            //                new ActiveDirectoryServiceSettings()
            //                {
            //                    AuthenticationEndpoint = new Uri("https://login.windows-ppe.net/"),
            //                    TokenAudience = new Uri("spn:e6182fa1-0f55-40b5-86ad-20a919e842c0"),
            //                    ValidateAuthority = true
            //                });

            //return new AzureTokenServiceAPI(clientCreds);


            var tokenServiceSettings = new ActiveDirectoryServiceSettings
            {
                //    AuthenticationEndpoint = new Uri("https://login.microsoftonline.com/"),
                //    TokenAudience = new Uri("spn:ec9a472d-515b-48a8-8699-ce105d20e9f9"),
                AuthenticationEndpoint = new Uri(_config["Values:AUTHENTICATION_ENDPOINT"]),
                TokenAudience = new Uri(_config["Values:TOKENAUDIENCE_URI"]),
                ValidateAuthority = true
            };

            var credentials = ApplicationTokenProvider.LoginSilentAsync(
                _config["Values:DOMAIN"],
                _config["Values:APPLICATIONID"],
                _config["Values:SECRET"],
                tokenServiceSettings).GetAwaiter().GetResult();

            return new AzureTokenServiceAPI(new Uri(_config["Values:SERVICEENDPOINT"]), credentials);
        }

        public TokenAPIService()
        {
            _config = readSettings();
        }

        private IConfigurationRoot _config = null;

        private IConfigurationRoot readSettings()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = null;


            if (File.Exists(Path.Combine(fileInfo.Directory.FullName, "application.settings.json")))
            {
                //Normal Application
                path = fileInfo.Directory.FullName;
            }
            else if (File.Exists(Path.Combine(fileInfo.Directory.Parent.FullName, "application.settings.json")))
            {
                //For Function App
                path = fileInfo.Directory.Parent.FullName;
            }
            else if (File.Exists(Path.Combine(fileInfo.Directory.FullName, "appsettings.json")))
            {
                //Normal Application
                path = fileInfo.Directory.FullName;
                //For ASP.net core
                return new ConfigurationBuilder()
                       .SetBasePath(path)
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .Build();
            }

            return new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("application.settings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}

