using CargoSmart.Windows.Booking.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CargoSmart.Windows.Booking.FieldConverter
{
    public class CargoNatureValueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case BookingRequestCargoNature._0:
                    return "Normal";
                case BookingRequestCargoNature._1:
                    return "Dangerous";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           switch (value)
            {
                case "Normal":
                    return BookingRequestCargoNature._0;
                case "Dangerous":
                    return BookingRequestCargoNature._1;

                default:
                    return null;
            }
        }
    }
}
