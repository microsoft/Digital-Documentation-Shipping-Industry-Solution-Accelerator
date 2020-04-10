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
                case "ContosoProductManager":
                    return "ContosoCargo";
                case "8b109889-0632-4658-9444-91f1c7b2e6f1":
                    return "Shipper A";
                case "ffa4b8e6-f634-4069-9b2d-e06749664808":
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
                    return "ContosoProductManager";
                case "Shipeer A":
                    return "8b109889-0632-4658-9444-91f1c7b2e6f1";
                case "Shipper B":
                    return "ffa4b8e6-f634-4069-9b2d-e06749664808";
                default:
                    return "";
            }
        }
    }
}
