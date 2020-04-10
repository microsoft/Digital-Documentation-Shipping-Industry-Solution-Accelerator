using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CargoSmart.Windows.Booking.ServiceProxy;

namespace CargoSmart.Windows.Booking.FieldConverter
{
    public class CurrentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CargoTokenShipmentCurrentStatus._0:
                    return "Quotation Requested";
                case CargoTokenShipmentCurrentStatus._1:
                    return "Quoted";
                case CargoTokenShipmentCurrentStatus._2:
                    return "Booking Reuqest";
                case CargoTokenShipmentCurrentStatus._3:
                    return "Booking Confirmed";
                case CargoTokenShipmentCurrentStatus._4:
                    return "Job Order Sent";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Quotation Requested":
                    return CargoTokenShipmentCurrentStatus._0;
                case "Quoted":
                    return CargoTokenShipmentCurrentStatus._1;
                case "Booking Reuqest":
                    return CargoTokenShipmentCurrentStatus._2;
                case "Booking Confirmed":
                    return CargoTokenShipmentCurrentStatus._3;
                case "Job Order Sent":
                    return CargoTokenShipmentCurrentStatus._4;
                default:
                    return null;
            }
        }
    }
}
