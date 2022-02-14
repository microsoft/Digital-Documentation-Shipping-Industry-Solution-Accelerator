// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Solutions.NFT;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoCargo.DigitalDocument.Setup
{
    internal class SetupApplication : IHostedService
    {
        ContosoCargoDigitalDocumentSetup setup;

        public SetupApplication(ContosoCargoDigitalDocumentSetup AppSetup)
        {
            setup = AppSetup;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SetupEnvironment(setup);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task SetupEnvironment(ContosoCargoDigitalDocumentSetup setup)
        {
            Console.WriteLine("\nStart to registering..........");

            Console.WriteLine("\nRegistering group.....");
            var party = await setup.SetupParty();
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering blockchian network.....");
            var bcNetwork = await setup.SetupBlockChainNetwork();
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering Contoso Cargo account.....");
            var contosoCargo = await setup.SetupContosoUsers("Contoso Cargo", party.Id, bcNetwork.Id);
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering Shipper A account.....");
            var shipperA = await setup.SetupContosoUsers("Shipper A", party.Id, bcNetwork.Id);
            Console.WriteLine("====> Done");

            Console.WriteLine("\nRegistering Shipper B account.....");
            var shipperB = await setup.SetupContosoUsers("Shiper B", party.Id, bcNetwork.Id);
            Console.WriteLine("====> Done");

            Console.WriteLine("\n\nAll Set process has been completed. " +
                            "\nUpdate your App.config file with below values in Windows Client Application");


            Console.WriteLine("\n===================   Configuration Information  =====================");
            Console.WriteLine($"Contoso Cargo Id\t: {contosoCargo.Id}");
            Console.WriteLine($"Shipper A Id\t\t: {shipperA.Id}");
            Console.WriteLine($"Shipper B Id\t\t: {shipperB.Id}");
            Console.WriteLine("======================================================================");

            Console.WriteLine("==> Copy these values then Hit Enter to close.");
            Console.ReadLine();
        }
    }


    public class ContosoCargoDigitalDocumentSetup
    {
        ServiceClient tokenServiceClient;
        IConfiguration config;

        public ContosoCargoDigitalDocumentSetup(IConfiguration Config, HttpClient HttpClient)
        {
            this.config = Config;
            tokenServiceClient = new ServiceClient(Config["Settings:TokenServiceEndpoint"], HttpClient);
        }

        public async Task<BlockchainNetwork> SetupBlockChainNetwork()
        {
            var result = await tokenServiceClient.RegisterBlockchainNetworkAsync(new BlockchainNetworkInfo()
            {
                Name = config["Settings:BlockchainNetworkName"],
                Description = config["Settings:BlockchainNetworkDescription"],
                NodeURL = config["Settings:BlockchainNetworkTxNode"]
            });

            return result;
        }

        public async Task<Party> SetupParty()
        {
            string _partyName = config["Settings:PartyName"];
            string _partyDescription = config["Settings:PartyDescription"];

            var result = await tokenServiceClient.RegisterPartyAsync(new PartyInfo()
            {
                PartyName = _partyName,
                Description = _partyDescription
            });

            return result;
        }


        public async Task<User> SetupContosoUsers(string UserName, Guid PartyID, Guid BlockchainNetworkID)
        {
            return await tokenServiceClient.RegisterUserAsync(new UserInfo()
            {
                Name = UserName,
                PartyID = PartyID,
                BlockchainNetworkID = BlockchainNetworkID
            });

        }
    }
}
