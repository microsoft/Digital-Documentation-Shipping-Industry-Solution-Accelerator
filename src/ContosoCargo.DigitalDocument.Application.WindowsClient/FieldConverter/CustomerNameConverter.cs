// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CargoSmart.Windows.Booking.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CargoSmart.Windows.Booking.FieldConverter
{
    public class CustomerNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = 
                User.GetUserAccounts().Where<User>(x => x.Address == value.ToString()).FirstOrDefault();

            return (user != null) ? user.UserName : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user =
                User.GetUserAccounts().Where<User>(x => x.UserName == value.ToString()).FirstOrDefault();

            return (user != null) ? user.Address : "";
        }
    }
}
