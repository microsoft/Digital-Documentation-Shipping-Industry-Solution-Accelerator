// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Configuration;

namespace CargoSmart.Windows.Booking.Entities
{
    public class User 
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public Role Role { get; set; }

        private static List<User> _users;

        public static List<User> GetUserAccounts()
        {
            if (User._users != null && _users.Count > 0) return _users;

            User._users = new List<User>();
            User._users.Add(new User() { UserName = "Contoso Cargo", Address = ConfigurationManager.AppSettings["ContosoCargo_Id"], Role = Role.Carrier });
            User._users.Add(new User() { UserName = "Shipper A", Address = ConfigurationManager.AppSettings["ShipperA_Id"], Role = Role.Shipper });
            User._users.Add(new User() { UserName = "Shipper B", Address = ConfigurationManager.AppSettings["ShipperB_Id"], Role = Role.Shipper });

            return User._users;
        }

    }

    public enum Role
    {
        Shipper,
        Carrier,
        Trucker
    }
}
