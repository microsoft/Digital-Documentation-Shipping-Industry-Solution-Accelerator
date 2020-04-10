using CargoSmart.Windows.Booking.ServiceProxy;
using CargoSmart.Windows.Booking.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Navigation;

namespace CargoSmart.Windows.Booking
{
    /// <summary>
    /// Interaction logic for Quotation.xaml
    /// </summary>
    public partial class Quotation : Window
    {
        private string _userAccount;
        private OrderManager _motherForm;
        private bool _isQuotationRequest;

        public Quotation(OrderManager motherForm, string userAccount, bool isQuotationRequest = false)
        {
            InitializeComponent();

            InitializeCombos();

            _userAccount = userAccount;
            _motherForm = motherForm;
            _isQuotationRequest = isQuotationRequest;

            CargoTokenShipment Shipment = motherForm.DataContext as CargoTokenShipment;

            if (!isQuotationRequest)
            {
                this.lblQuotationRequestTitle.Content = "Quotation Title :";
                this.lblQuotationDescription.Content = "Quotation Description : ";
                this.Title = "Send Quotation";

                QuoteInfo quoteInfo = JsonConvert.DeserializeObject<QuoteInfo>(Shipment.QuoteRequestTokenInfo.BusinessMetaData);
                txtFrom.Text = quoteInfo.From;
                txtTo.Text = quoteInfo.To;
                dpCargoReadyDate.SelectedDate = quoteInfo.CargoReadyDate.GetValueOrDefault().UtcDateTime;
                cboCargoNature.Text = quoteInfo.CargoNature;
                cboContainerSize.SelectedIndex = (int)quoteInfo.ContainerSize; //Enum.GetName//quoteInfo.ContainerSize
                cboContainerType.SelectedIndex = (int)quoteInfo.ContainerType;
            }
        }

        public Quotation()
        {
            InitializeComponent();
            InitializeCombos();
        }


        private void InitializeCombos()
        {
            SetupUserAccountCombo();
            SetupCargoNatureCombo();
            SetupcboContainerSize();
            SetupcboContainerType();
        }

        private void SetupCargoNatureCombo()
        {
            cboCargoNature.Items.Add("Normal");
            cboCargoNature.Items.Add("Dangerous");

            cboCargoNature.SelectedIndex = 0;
        }

        private void SetupcboContainerSize()
        {
            cboContainerType.Items.Add("Large");
            cboContainerType.Items.Add("Medium");
            cboContainerType.Items.Add("Small");

            cboContainerSize.SelectedIndex = 0;
        }

        private void SetupcboContainerType()
        {
            cboContainerSize.Items.Add("Type A");
            cboContainerSize.Items.Add("Type B");
            cboContainerSize.Items.Add("Type C");

            cboContainerType.SelectedIndex = 0;
        }

        private void SetupUserAccountCombo()
        {
            cboCustomer.ItemsSource = User.GetUserAccounts().Where<User>(x => x.Role.Equals(Role.Carrier));
            cboCustomer.DisplayMemberPath = "UserName";
            cboCustomer.SelectedValuePath = "Address";

            cboCustomer.SelectedIndex = 0;
        }

        private async Task<CargoTokenShipment> CreateQuotationRequest()
        {
            HttpServiceProxy proxy = HttpServiceLocator.GetHttpServiceProxy(); //new HttpServiceProxy(new System.Net.Http.HttpClient());

            CargoTokenShipment Shipment = null;

            if (!_isQuotationRequest) { Shipment = _motherForm.DataContext as CargoTokenShipment; }

            QuoteRequestMessage quoteRequestMessage = new QuoteRequestMessage()
            {
                ShipmentID = _isQuotationRequest ? "" : Shipment.Id.ToString(),
                CallerID = ((User)cboCustomer.SelectedItem).Address,
                CustomerID = _isQuotationRequest ? _userAccount : Shipment.Contractee,
                QuoteTitle = txtQuotationTitle.Text,
                QuoteDescription = txtQuotationDescription.Text,
                QuoteInfo = new QuoteInfo()
                {
                    From = txtFrom.Text,
                    To = txtTo.Text,
                    CargoReadyDate = dpCargoReadyDate.SelectedDate,
                    CargoNature = cboCargoNature.Text,
                    ContainerSize = (QuoteInfoContainerSize)Enum.GetValues(typeof(QuoteInfoContainerSize)).GetValue(cboContainerSize.SelectedIndex), //QuoteInfoContainerSize._0,
                    ContainerType = (QuoteInfoContainerType)Enum.GetValues(typeof(QuoteInfoContainerType)).GetValue(cboContainerType.SelectedIndex)//QuoteInfoContainerType._0
                }
            };

            if (_isQuotationRequest)
            {
                return await proxy.CreateQuoteRequestAsync(quoteRequestMessage);
            }
            else
            {
                return await proxy.CreateQuoteAsync(quoteRequestMessage);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (new WaitCursor(_motherForm.pgbProgress))
            {
                CargoTokenShipment _Shipment = await CreateQuotationRequest();

                int lstIndex = 0;

                if (!_isQuotationRequest) { lstIndex = _motherForm.lstShipments.SelectedIndex; }

                await _motherForm.GetMyShipment(true);

                if (!_isQuotationRequest) { _motherForm.lstShipments.SelectedItem = lstIndex; }

                Debug.WriteLine(JsonConvert.SerializeObject(_Shipment));
            }

            this.Close();
        }
    }
}
