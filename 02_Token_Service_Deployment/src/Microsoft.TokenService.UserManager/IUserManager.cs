using Microsoft.TokenService.UserManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.UserManager
{
    public interface IUserManager
    {
        User GetUser(Guid Id);
        Task<User> RegisterUser(string Name, string Description, Guid PartyID, Guid BlockchainNetworkID);
        void UnRegistUser(Guid id);
        IEnumerable<User> GetAllUsers();
    }
}