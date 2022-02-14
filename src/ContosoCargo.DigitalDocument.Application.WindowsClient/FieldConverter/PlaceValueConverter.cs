// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CargoSmart.Windows.Booking.ServiceProxy;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CargoSmart.Windows.Booking.FieldConverter
{
    public class PlaceValueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case BookingRequestPlace._0:
                    return "Yard";
                case BookingRequestPlace._1:
                    return "Door";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            switch (value)
            {
                case "Yard":
                    return BookingRequestPlace._0;

                case "Door":
                    return BookingRequestPlace._1;
                default:
                    return null;
            }
        }
    }
}
