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
            users.Add(new User() { UserName = "Contoso Cargo", Address = "832a88be-6087-4127-8b8f-515a0704e973", Role = Role.Carrier });
            users.Add(new User() { UserName = "Shipper A", Address = "cb98ab5a-3c2c-451f-9cc8-54f02c2ebfb2", Role = Role.Shipper });
            users.Add(new User() { UserName = "Shipper B", Address = "95ebd855-b54b-4f1a-99ac-5934a71f1440", Role = Role.Shipper });

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
