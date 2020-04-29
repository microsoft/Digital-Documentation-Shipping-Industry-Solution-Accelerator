using ContosoCargo.DigitalDocument.TokenService.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using ContosoCargo.DigitalDocument.TokenService.Client;
using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo;
using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase;
using ContosoCargo.DigitalDocument.TokenService.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ContosoCargo.DigitalDocument.TokenService.Host.Application;
using System;
using System.IO;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ContosoCargo.DigitalDocument.TokenService.Host
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services).BuildServiceProvider(true);
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {

            var mongoConnString = Startup._config["App:Offchain_Connectionstring"];
            

            services
                .AddTransient<IContosoCargoApplication, Application.ContosoCargo>(c =>
               {
                   var repo = new BusinessTransactionRepository<CargoTokenShipment, Guid>(new MongoClient(mongoConnString), "CargoTokenRepository");
                   return new Application.ContosoCargo(repo, Startup._config["App:EndpointURL"]);
               });

            return services;
        }

        public Startup()
        {
            if (Startup._config != null) return;
            Startup._config = readSettings();
        }


        private static IConfigurationRoot _config = null;

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
            return new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("application.settings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
