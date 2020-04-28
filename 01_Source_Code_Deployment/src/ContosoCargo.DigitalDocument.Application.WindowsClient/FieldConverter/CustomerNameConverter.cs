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
            switch (value)
            {
                case "0267c462-f629-45a5-b140-df50c6b219bd":
                    return "Contoso Cargo";
                case "c466d31d-f669-4d99-9e02-551ef8284c11":
                    return "Shipper A";
                case "03502024-a6ae-4c8c-b87a-56da9c262a77":
                    return "Shipper B";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Contoso Cargo":
                    return "0267c462-f629-45a5-b140-df50c6b219bd";
                case "Shipper A":
                    return "c466d31d-f669-4d99-9e02-551ef8284c11";
                case "Shipper B":
                    return "03502024-a6ae-4c8c-b87a-56da9c262a77";
                default:
                    return "";
            }
        }
    }
}
