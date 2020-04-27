using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TokenService.API.MessageBags;
using Microsoft.TokenService.BlockchainNetworkManager;
using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Microsoft.TokenService.PartyManager;
using Microsoft.TokenService.PartyManager.Model;
using Microsoft.TokenService.UserManager;
using Microsoft.TokenService.UserManager.Model;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace Microsoft.TokenService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceManagementController : ControllerBase
    {
        private IBlockchainNetworkManager blockchainNetworkManager;
        private IPartyManager partyManager;
        private IUserManager userManager;

        public ServiceManagementController(IBlockchainNetworkManager blockchains, IPartyManager parties, IUserManager users)
        {
            blockchainNetworkManager = blockchains;
            partyManager = parties;
            userManager = users;
        }


        [HttpPost]
        [Route("BlockchainNetworks/BlockchainNetwork/{Id}")]
        public BlockchainNetwork GetBlockchainNetwork(Guid Id)
        {
            return blockchainNetworkManager.GetBlockchainNetwork(Id);
        }

        [HttpPost]
        [Route("BlockchainNetworks")]
        public IEnumerable<BlockchainNetwork> GetAllBlockchainNetwork()
        {
            return blockchainNetworkManager.GetAllBlockchainNetworks();
        }


        [HttpPost]
        [Route("BlockchainNetworks/BlockchainNetwork")]
        public async Task<BlockchainNetwork> RegisterBlockchainNetwork([FromBody] MessageBags.BlockchainNetworkInfo BlockchainNetworkInfo)
        {


            return await blockchainNetworkManager.RegisterBlockchainNetwork(BlockchainNetworkInfo.Name,
                                                                            BlockchainNetworkInfo.NodeURL,
                                                                            BlockchainNetworkInfo.Description);
        }


        [HttpDelete]
        [Route("BlockchainNetworks/BlockchainNetwork/{Id}")]
        public bool UnregisterBlockchainNetwork(Guid Id)
        {
            try
            {
                blockchainNetworkManager.UnRegisterBlockchainNetwork(Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        [Route("Parties/Party/{Id}")]
        public Party GetParty(Guid Id)
        {
            return partyManager.GetParty(Id);
        }

        [HttpPost]
        [Route("Parties")]
        public IEnumerable<Party> GetParty()
        {
            return partyManager.GetAllParty();
        }

        [HttpPost]
        [Route("Parties/Party")]
        public async Task<Party> RegisterParty([FromBody] PartyInfo PartyInfo)
        {
            return await partyManager.RegisterParty(PartyInfo.PartyName, PartyInfo.Description);
        }

        [HttpDelete]
        [Route("Parties/Party/{Id}")]
        public bool UnregisterParty(Guid Id)
        {
            try
            {
                partyManager.UnRegisterParty(Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        [Route("Users/User/{Id}")]
        public User GetUser(Guid Id)
        {
            return userManager.GetUser(Id);
        }


        [HttpPost]
        [Route("Users/User")]
        public async Task<User> RegisterUser([FromBody] UserInfo UserInfo)
        {
            return await userManager.RegisterUser(UserInfo.Name,
                                                    UserInfo.Description,
                                                    UserInfo.PartyID,
                                                    UserInfo.BlockchainNetworkID);
        }

        [HttpPost]
        [Route("Users")]
        public IEnumerable<User> GetAllUsers()
        {
            return userManager.GetAllUsers();
        }


        [HttpDelete]
        [Route("Users/User/{Id}")]
        public bool UnRegisterUser(Guid Id)
        {
            try
            {
                userManager.UnRegistUser(Id);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
