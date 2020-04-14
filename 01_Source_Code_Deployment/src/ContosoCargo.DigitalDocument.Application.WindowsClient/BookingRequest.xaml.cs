using CargoSmart.Windows.Booking.FieldConverter;
using CargoSmart.Windows.Booking.ServiceProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CargoSmart.Windows.Booking
{
    /// <summary>
    /// Interaction logic for BookingRequest.xaml
    /// </summary>
    public partial class BookingRequest : Window
    {
        private OrderManager _motherForm;
        private CargoTokenShipment _Shipment;
        public string ShipperAccount { get; set; }


        public BookingRequest()
        {
              InitializeComponent();
        }

        public BookingRequest(OrderManager motherForm, CargoTokenShipment ShipmentInfo) : this()
        {
            _motherForm = motherForm;
            _Shipment = ShipmentInfo;


            pnlCustomerName.DataContext = _Shipment;

            //this.ShipperAccount = _Shipment.Contracter;

            grdBookingRequest.DataContext = JsonConvert.DeserializeObject<QuoteInfo>(ShipmentInfo.QuotedTokenInfo.BusinessMetaData);

            SetupShippingPlaceCombo();
        }

        private void SetupShippingPlaceCombo()
        {
            cboShippingplace.Items.Add("Yard");
            cboShippingplace.Items.Add("Door");

            cboShippingplace.SelectedIndex = 0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (new WaitCursor(_motherForm.pgbProgress))
            {
                await CreateBookingRequest();
                await _motherForm.GetMyShipment(true);
            }

            this.Close();
        }

        private async Task CreateBookingRequest()
        {
            HttpServiceProxy httpServiceProxy = HttpServiceLocator.GetHttpServiceProxy();

            QuoteInfo quoteInfo = JsonConvert.DeserializeObject<QuoteInfo>(_Shipment.QuotedTokenInfo.BusinessMetaData);

            BookingRequestMessage bookingRequestInfo = new BookingRequestMessage()
            {
                ShipmentID = _Shipment.Id.ToString(),
                BookingRequestTitle = txtTitle.Text,
                BookingRequestDescription = txtDescription.Text,
                CallerID = _Shipment.Contracter,
                CustomerID = _Shipment.Contractee,
                BookingRequestInfo = new BookingRequestInfo()
                {
                    From = quoteInfo.From,
                    To = quoteInfo.To,
                    ContainerSize =  (BookingRequestInfoContainerSize)(quoteInfo.ContainerSize.GetValueOrDefault()), 
                    ContainerType = (BookingRequestInfoContainerType)(quoteInfo.ContainerType.GetValueOrDefault()), 
                    CargoNature = (BookingRequestInfoCargoNature)(new CargoNatureValueTypeConverter()).ConvertBack(quoteInfo.CargoNature, typeof(BookingRequestInfoCargoNature), null, null), 
                    Place = (BookingRequestInfoPlace)(cboShippingplace.SelectedIndex),
                    Quantity = int.Parse(txtQuantity.Text),
                    Shipper = _Shipment.Contractee,
                    Weight = int.Parse(txtWeight.Text)
                }
            };

            var result = await httpServiceProxy.CreateBookingRequestAsync(_Shipment.Id.ToString(), bookingRequestInfo);
            Debug.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
