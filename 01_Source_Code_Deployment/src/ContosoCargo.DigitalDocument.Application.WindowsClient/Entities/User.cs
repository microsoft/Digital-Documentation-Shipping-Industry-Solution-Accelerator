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
            users.Add(new User() { UserName = "OOCL", Address = "ContosoProductManager", Role = Role.Carrier });
            users.Add(new User() { UserName = "Shipper A", Address = "8b109889-0632-4658-9444-91f1c7b2e6f1", Role = Role.Shipper });
            users.Add(new User() { UserName = "Shipper B", Address = "ffa4b8e6-f634-4069-9b2d-e06749664808", Role = Role.Shipper });

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
