using Microsoft.Extensions.Configuration;
using Microsoft.TokenService.Proxy;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ContosoCargo.DigitalDocument.Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            ContosoCargoDigitalDocumentSetup setup = new ContosoCargoDigitalDocumentSetup(ConfigurationLoader.Config);
            Console.WriteLine("\nStart to registering..........");

            Console.WriteLine("\nRegistering group.....");
            var party = setup.SetupParty().Result;
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering blockchian network.....");
            var bcNetwork = setup.SetupBlockChainNetwork().Result;
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering Contoso Cargo account.....");
            var contosoCargo = setup.SetupContosoUsers("Contoso Cargo", party.Id , bcNetwork.Id).Result;
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering Shipper A account.....");
            var shipperA = setup.SetupContosoUsers("Shipper A", party.Id, bcNetwork.Id).Result;
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering Shipper B account.....");
            var shipperB = setup.SetupContosoUsers("Shiper B", party.Id, bcNetwork.Id).Result;
            Console.WriteLine("====> Done");

            Console.WriteLine("All Set process has been completed. " +
                            "\nUpdate your User.cs and CustomerNameConverter.cs file  in Windows Client Application");


            Console.WriteLine("\n===================   Configuration Information  =====================");
            Console.WriteLine($"Contoso Cargo Address\t: {contosoCargo.Id}");
            Console.WriteLine($"Shipper A Address\t: {shipperA.Id}");
            Console.WriteLine($"Shipper B Address\t: {shipperB.Id}");
            Console.WriteLine("======================================================================");

            Console.WriteLine("==> Copy these values then Hit Enter to close.");
            Console.ReadLine();

        }
    }



    class ContosoCargoDigitalDocumentSetup
    {
        private IConfigurationRoot config;
        public ContosoCargoDigitalDocumentSetup(IConfigurationRoot Config)
        {
            this.config = Config;
        }

        public async Task<BlockchainNetwork> SetupBlockChainNetwork()
        {
            BlockchainNetworksClient bcClient = new BlockchainNetworksClient(config["Settings:TokenServiceEndpoint"]);

            var result = await bcClient.BlockchainNetworkPostAsync(new BlockchainNetworkInfo()
            {
                Name = config["Settings:BlockchainNetworkName"],
                Description = config["Settings:BlockchainNetworkDescription"],
                NodeURL = config["Settings:BlockchainNetworkTxNode"]
            });

            return result;
        }

        public async Task<Party> SetupParty()
        {
            string serviceEndpoint = config["Settings:TokenServiceEndpoint"];
            string _partyName = config["Settings:PartyName"];
            string _partyDescription = config["Settings:PartyDescription"];

            PartiesClient partyClient = new PartiesClient(serviceEndpoint);

            var result = await partyClient.PartyPostAsync(new PartyInfo()
            {
                PartyName = _partyName,
                Description = _partyDescription
            });

            return result;
        }


        public async Task<User> SetupContosoUsers(string UserName, Guid PartyID, Guid BlockchainNetworkID)
        {
            string serviceEndpoint = config["Settings:TokenServiceEndpoint"];
            UsersClient usersClient = new UsersClient(serviceEndpoint);

            return await usersClient.UserPostAsync(new UserInfo()
            {
                Name = UserName,
                PartyID = PartyID,
                BlockchainNetworkID = BlockchainNetworkID
            });

        }
    }


}


