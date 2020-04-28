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
            users.Add(new User() { UserName = "Contoso Cargo", Address = "0267c462-f629-45a5-b140-df50c6b219bd", Role = Role.Carrier });
            users.Add(new User() { UserName = "Shipper A", Address = "c466d31d-f669-4d99-9e02-551ef8284c11", Role = Role.Shipper });
            users.Add(new User() { UserName = "Shipper B", Address = "03502024-a6ae-4c8c-b87a-56da9c262a77", Role = Role.Shipper });

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
