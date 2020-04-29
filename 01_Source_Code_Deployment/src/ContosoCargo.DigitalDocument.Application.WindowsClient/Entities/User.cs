using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoSmart.Windows.Booking.Entities
{
    public class User
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public Role Role { get; set; }

        public static List<User> GetUserAccounts()
        {
            List<User> users = new List<User>();
            users.Add(new User() { UserName = "Contoso Cargo", Address = "252e1855-4e6c-4c78-b43e-c2ef1b3ac31f", Role = Role.Carrier });
            users.Add(new User() { UserName = "Shipper A", Address = "fef36ba9-d1f4-43a9-b985-65fda10c17ee", Role = Role.Shipper });
            users.Add(new User() { UserName = "Shipper B", Address = "7656fd27-10e8-456b-84f5-9463dac5a38e", Role = Role.Shipper });

            return users;
        }

    }

    public enum Role
    {
        Shipper,
        Carrier,
        Trucker
    }
}
