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
                case "252e1855-4e6c-4c78-b43e-c2ef1b3ac31f":
                    return "ContosoCargo";
                case "fef36ba9-d1f4-43a9-b985-65fda10c17ee":
                    return "Shipper A";
                case "7656fd27-10e8-456b-84f5-9463dac5a38e":
                    return "Shipper B";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "ContosoCargo":
                    return "252e1855-4e6c-4c78-b43e-c2ef1b3ac31f";
                case "Shipper A":
                    return "fef36ba9-d1f4-43a9-b985-65fda10c17ee";
                case "Shipper B":
                    return "7656fd27-10e8-456b-84f5-9463dac5a38e";
                default:
                    return "";
            }
        }
    }
}
