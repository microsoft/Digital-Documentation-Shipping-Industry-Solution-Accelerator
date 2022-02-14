// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using CargoSmart.Windows.Booking.ServiceProxy;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace CargoSmart.Windows.Booking
{
    /// <summary>
    /// Interaction logic for BookingConfirmation.xaml
    /// </summary>
    public partial class BookingConfirmation : Window
    {
        private OrderManager _motherForm;
        private CargoTokenShipment _Shipment;

        public BookingConfirmation()
        {
            InitializeComponent();
        }

        public BookingConfirmation(OrderManager motherForm, CargoTokenShipment ShipmentInfo) : this()
        {
            _motherForm = motherForm;
            _Shipment = ShipmentInfo;

            ServiceProxy.BookingRequest bookingRequest =
               JsonConvert.DeserializeObject<ServiceProxy.BookingRequest>(_Shipment.BookingRequestTokenInfo.BusinessMetaData);

            pnlCustomerName.DataContext = _Shipment;
            grdBookingConfirmation.DataContext = bookingRequest;
        }

        private async Task<CargoTokenShipment> CreateBookingConfirmation()
        {
            HttpServiceProxy proxy = HttpServiceLocator.GetHttpServiceProxy();

            BookingConfirmationRequestMessage bookingConfirmationRequestMessage = new BookingConfirmationRequestMessage()
            {
                ShipmentID = _Shipment.Id.ToString(),
                CallerID = _Shipment.Contracter,
                CustomerID = _Shipment.Contractee,
                BookingConfirmationTitle = txtTitle.Text,
                BookingConfirmationDescription = txtDescription.Text,
                BookingConfirmationInfo = new BookingConfirmationInfo()
                {
                    CyCutOff  = txtCYCutOff.Text,
                    SiCutOff = txtCICutOff.Text,
                    EmptyContainerPickupLocation = txtEmptyContainerPickupLocation.Text,
                    LadenContainerReturnLocation = txtLadenContainerPickupLocation.Text,
                    BookingRequest = (ServiceProxy.BookingRequest)grdBookingConfirmation.DataContext
                }
            };

            var result = await proxy.CreateBookingConfirmationAsync(_Shipment.Id.ToString(), bookingConfirmationRequestMessage);
            Debug.WriteLine(result);

            return result;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (new WaitCursor(_motherForm.pgbProgress))
            {
                await CreateBookingConfirmation();
                await _motherForm.GetMyShipment(true);
            }
            this.Close();
        }
    }
}
