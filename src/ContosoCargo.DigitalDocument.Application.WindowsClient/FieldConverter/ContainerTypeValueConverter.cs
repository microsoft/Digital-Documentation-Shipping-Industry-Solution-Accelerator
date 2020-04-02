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
    public class ContainerTypeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case QuoteInfoContainerType._0:
                case BookingRequestContainerType._0:
                    return "Type A";
                case QuoteInfoContainerType._1:
                case BookingRequestContainerType._1:
                    return "Type B";
                case QuoteInfoContainerType._2:
                case BookingRequestContainerType._2:
                    return "Type C";
                default:
                    return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Type A":
                    if (targetType == typeof(BookingRequestContainerType))
                    {
                        return BookingRequestContainerType._0;
                    }

                    return null;

                case "Type B":
                    if (targetType == typeof(BookingRequestContainerType))
                    {
                        return BookingRequestContainerType._1;
                    }
                    return null;
                case "Type C":
                    if (targetType == typeof(BookingRequestContainerType))
                    {
                        return BookingRequestContainerType._2;
                    }
                    return null;
                default:
                    return null;
            }
        }
    }
}
