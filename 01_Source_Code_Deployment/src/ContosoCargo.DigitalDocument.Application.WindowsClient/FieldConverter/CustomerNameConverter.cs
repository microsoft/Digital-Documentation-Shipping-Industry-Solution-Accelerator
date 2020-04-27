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
                case "3700b3d9-afa0-41d8-9bb5-b7834e2745ce":
                    return "Contoso Cargo";
                case "648e1353-eefc-4fa3-b074-d30c45c1b536":
                    return "Shipper A";
                case "8203eb14-218e-41cc-a01a-917d69f6db1b":
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
                    return "3700b3d9-afa0-41d8-9bb5-b7834e2745ce";
                case "Shipper A":
                    return "8203eb14-218e-41cc-a01a-917d69f6db1b";
                case "Shipper B":
                    return "3adfcbc9-5fe1-430e-abca-e53a4e4c42e4";
                default:
                    return "";
            }
        }
    }
}
