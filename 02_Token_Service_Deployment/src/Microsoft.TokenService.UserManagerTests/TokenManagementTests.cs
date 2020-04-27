using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TokenService.UserManager;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Microsoft.TokenService.PartyManager.Model;
using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Microsoft.TokenService.UserManager.Model;

namespace Microsoft.TokenService.Management.Tests
{
    [TestClass()]
    public class TokenManagementTests
    {
        IConfigurationRoot _config;
        static Party _newParty;
        static BlockchainNetwork _blockchainNetwork;
        static User _user;

        [TestInitialize()]
        public void InitTest()
        {
            _config = ConfigReader.ReadSettings();
        }


        [TestMethod()]
        public async Task Test01_CreateGroup()
        {
            Microsoft.TokenService.PartyManager.Parties party =
                new TokenService.PartyManager.Parties(_config["Offchain_connectionstring"], "Management");
            _newParty = await party.RegisterParty("Foo Party", "Description for Foo");

            Console.WriteLine($"Party has been created. \nPartyID : {_newParty.Id} \nPartyName : {_newParty.PartyName}");

            Assert.IsNotNull(_newParty);
        }

        [TestMethod()]
        public async Task Test02_CreateBlockchainNetwork()
        {
            Microsoft.TokenService.BlockchainNetworkManager.BlockchainNetworks blockchainNetwork =
              new BlockchainNetworkManager.BlockchainNetworks(_config["Offchain_connectionstring"], "Management");
            _blockchainNetwork = await blockchainNetwork.RegisterBlockchainNetwork("Foo network", "http://foo", "blabla");

            Console.WriteLine($"BlockhChainNetwork has been created. \nBlockchainNetworkID : {_blockchainNetwork.Id} \nBlockchainNetworkName : {_blockchainNetwork.Name}");

            Assert.IsNotNull(_blockchainNetwork);
        }

        [TestMethod()]
        public async Task Test03_CreateUser()
        {
            Microsoft.TokenService.UserManager.Users userManager =
                         new Users(_config["Offchain_connectionstring"], "Management", _config);

            _user = await userManager.RegisterUser("Nerde", "bla bla", _newParty.Id, _blockchainNetwork.Id);

            Console.WriteLine($"User has been created. \nUserID : {_user.Id} \nUserName : {_user.Name} \nPublicAddress : {_user.PublicAddress}");

            Assert.IsNotNull(_user);
        }

        [TestMethod()]
        public void Test04_GetParty()
        {
            Microsoft.TokenService.PartyManager.Parties party =
                new TokenService.PartyManager.Parties(_config["Offchain_connectionstring"], "Management");
            _newParty = party.GetParty(_newParty.Id);

            Console.WriteLine($"Party has been retrieved. \nPartyID : {_newParty.Id} \nPartyName : {_newParty.PartyName}");

            Assert.IsNotNull(_newParty);
        }

        [TestMethod()]
        public void Test05_GetBlockchainNetwork()
        {
            Microsoft.TokenService.BlockchainNetworkManager.BlockchainNetworks blockchainNetwork =
              new BlockchainNetworkManager.BlockchainNetworks(_config["Offchain_connectionstring"], "Management");
            _blockchainNetwork = blockchainNetwork.GetBlockchainNetwork(_blockchainNetwork.Id);

            Console.WriteLine($"BlockhChainNetwork has been retrieved. \nBlockchainNetworkID : {_blockchainNetwork.Id} \nBlockchainNetworkName : {_blockchainNetwork.Name}");

            Assert.IsNotNull(_blockchainNetwork);
        }

        [TestMethod()]
        public void Test06_GetUser()
        {
            Microsoft.TokenService.UserManager.Users userManager =
                       new Users(_config["Offchain_connectionstring"], "Management", _config);

            _user = userManager.GetUser(_user.Id);
            Console.WriteLine($"User has been retrieved. \nUserID : {_user.Id} \nUserName : {_user.Name} \nPublicAddress : {_user.PublicAddress}");

            Assert.IsNotNull(_user);
        }

        [TestMethod()]
        public void Test07_DeleteGroup()
        {
            Microsoft.TokenService.PartyManager.Parties party =
                new TokenService.PartyManager.Parties(_config["Offchain_connectionstring"], "Management");
            party.UnRegisterParty(_newParty.Id);

        }

        [TestMethod()]
        public void Test08_DeleteBlockchainNetwork()
        {
            Microsoft.TokenService.BlockchainNetworkManager.BlockchainNetworks blockchainNetwork =
              new BlockchainNetworkManager.BlockchainNetworks(_config["Offchain_connectionstring"], "Management");
            blockchainNetwork.UnRegisterBlockchainNetwork(_blockchainNetwork.Id);
        }

        [TestMethod()]
        public void Test09_DeleteUser()
        {
            Microsoft.TokenService.UserManager.Users userManager =
                       new Users(_config["Offchain_connectionstring"], "Management", _config);

            userManager.UnRegistUser(_user.Id);
        }
    }


    public class ConfigReader
    {
        public ConfigReader()
        {
        }


        public static IConfigurationRoot ReadSettings()
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