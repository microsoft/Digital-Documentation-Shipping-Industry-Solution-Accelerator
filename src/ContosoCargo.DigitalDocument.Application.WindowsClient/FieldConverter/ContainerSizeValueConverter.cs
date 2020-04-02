using CargoSmart.Windows.Booking.ServiceProxy;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CargoSmart.Windows.Booking.FieldConverter
{
    public class ContainerSizeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case QuoteInfoContainerSize._0:
                case BookingRequestContainerSize._0:
                    return "Large";
                case QuoteInfoContainerSize._1:
                case BookingRequestContainerSize._1:
                    return "Medium";
                case QuoteInfoContainerSize._2:
                case BookingRequestContainerSize._2:
                    return "Small";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            switch (value)
            {
                case "Large":
                    if (targetType == typeof(BookingRequestInfoContainerSize))
                    {
                        return BookingRequestInfoContainerSize._0;
                    }

                    return null;

                case "Medium":
                    if (targetType == typeof(BookingRequestInfoContainerSize))
                    {
                        return BookingRequestInfoContainerSize._1;
                    }
                    return null;
                case "Small":
                    //return "Small";
                    if (targetType == typeof(BookingRequestInfoContainerSize))
                    {
                        return BookingRequestInfoContainerSize._2;
                    }
                    return null;
                default:
                    return null;
            }
        }
    }
}
