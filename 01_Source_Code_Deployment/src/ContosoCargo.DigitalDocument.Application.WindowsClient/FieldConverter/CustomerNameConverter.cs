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
                case "832a88be-6087-4127-8b8f-515a0704e973":
                    return "Contoso Cargo";
                case "cb98ab5a-3c2c-451f-9cc8-54f02c2ebfb2":
                    return "Shipper A";
                case "95ebd855-b54b-4f1a-99ac-5934a71f1440":
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
                    return "832a88be-6087-4127-8b8f-515a0704e973";
                case "Shipper A":
                    return "cb98ab5a-3c2c-451f-9cc8-54f02c2ebfb2";
                case "Shipper B":
                    return "95ebd855-b54b-4f1a-99ac-5934a71f1440";
                default:
                    return "";
            }
        }
    }
}
