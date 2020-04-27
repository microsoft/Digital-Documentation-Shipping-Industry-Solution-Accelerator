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
            users.Add(new User() { UserName = "Contoso Cargo", Address = "3700b3d9-afa0-41d8-9bb5-b7834e2745ce", Role = Role.Carrier });
            users.Add(new User() { UserName = "Shipper A", Address = "8203eb14-218e-41cc-a01a-917d69f6db1b", Role = Role.Shipper });
            users.Add(new User() { UserName = "Shipper B", Address = "3adfcbc9-5fe1-430e-abca-e53a4e4c42e4", Role = Role.Shipper });

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
