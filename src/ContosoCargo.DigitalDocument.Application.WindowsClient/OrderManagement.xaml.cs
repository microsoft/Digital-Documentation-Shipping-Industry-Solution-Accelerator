using CargoSmart.Windows.Booking.Entities;
using CargoSmart.Windows.Booking.ServiceProxy;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CargoSmart.Windows.Booking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OrderManager : Window
    {
        public OrderManager()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            SetupUserAccountCombo();
            SetupFilterCombo();
        }

        private void SetupFilterCombo()
        {
            cboFilter.Items.Add("All");
            cboFilter.Items.Add("Quote Request");
            cboFilter.Items.Add("Quoted");
            cboFilter.Items.Add("Booking Request");
            cboFilter.Items.Add("Booked");
            cboFilter.SelectedIndex = 0;
        }

        private void SetupUserAccountCombo()
        {
            cboUser.ItemsSource = User.GetUserAccounts();
            cboUser.DisplayMemberPath = "UserName";
            cboUser.SelectedValuePath = "Address";

            cboUser.SelectedIndex = 0;
        }

        public async Task GetMyShipment(bool IsRefresh = false)
        {
            HttpServiceProxy proxy = HttpServiceLocator.GetHttpServiceProxy();
            User selectedItem = cboUser.SelectedItem as User;

            if (selectedItem != null)
            {
                var result =
                      await proxy.GetMyShipmentsAsync(new GetTokenShipmentsRequest() { CallerID = selectedItem.Address, IsContracter = (selectedItem.Role == Role.Carrier) });

                lstShipments.ItemsSource = result;

                if (result.Count > 0)
                {
                    if (IsRefresh)
                    {
                        lstShipments.SelectedIndex = lstShipments.Items.Count - 1;
                    }
                    else
                    {
                        lstShipments.SelectedIndex = 0;
                    }

                    tabProcess.Visibility = Visibility.Visible;
                    lblCreateQuoteRequest.Visibility = Visibility.Collapsed;
                } else
                {
                    tabProcess.Visibility = Visibility.Collapsed;
                    lblCreateQuoteRequest.Visibility = Visibility.Visible;
                }
            }

        }

        private void lstShipments_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (lstShipments.SelectedItem == null) return;
        

            CargoTokenShipment Shipment = lstShipments.SelectedItem as CargoTokenShipment;

            this.DataContext = Shipment;

            //Activate Tab by Current Status
            this.tabProcess.SelectedIndex = (int)Shipment.CurrentStatus;


            //if(Shipment.QuoteRequestTokenInfo == null)



            //Control Tab visibility by Status
            this.tabQuoteRequest.Visibility = (Shipment.QuoteRequestTokenInfo == null) ? Visibility.Hidden : Visibility.Visible;
            this.tabQuote.Visibility = (Shipment.QuotedTokenInfo == null) ? Visibility.Hidden : Visibility.Visible;
            this.tabBookingRequest.Visibility = (Shipment.BookingRequestTokenInfo == null) ? Visibility.Hidden : Visibility.Visible;
            this.tabBookingConfirmation.Visibility = (Shipment.BookedTokenInfo == null) ? Visibility.Hidden : Visibility.Visible;

            if (Shipment.QuoteRequestTokenInfo != null)
            {
                //just in case
                if (string.IsNullOrEmpty(Shipment.QuoteRequestTokenInfo.BusinessMetaData)) return;

                //Deserialize business entity and set as DataContext
                QuoteInfo quoteRequest = JsonConvert.DeserializeObject<QuoteInfo>(Shipment.QuoteRequestTokenInfo.BusinessMetaData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                this.QuotationRequestDataEntityGrid.DataContext = quoteRequest;

                //Control button's visibility
                //CreateQuotation button need to show to Contracter
                //Shoudn't be seen in post status
                if (Shipment.Contracter == (cboUser.SelectedItem as User).Address)
                {
                    btnCreateQuotation.Visibility = Visibility.Visible;

                    _ = (Shipment.CurrentStatus > CargoTokenShipmentCurrentStatus._0) ? btnCreateQuotation.Visibility = Visibility.Collapsed
                                                                                            : btnCreateQuotation.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCreateQuotation.Visibility = Visibility.Collapsed;
                }
            }


            if (Shipment.QuotedTokenInfo != null)
            {
                if (string.IsNullOrEmpty(Shipment.QuotedTokenInfo.BusinessMetaData)) return;

                QuoteInfo quoteInfo = JsonConvert.DeserializeObject<QuoteInfo>(Shipment.QuotedTokenInfo.BusinessMetaData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                this.QuotationDataEntityGrid.DataContext = quoteInfo;



                //show button to Contractee
                if (Shipment.Contractee == (cboUser.SelectedItem as User).Address)
                {
                    btnCreateBookingRequest.Visibility = Visibility.Visible;
                    _ = (Shipment.CurrentStatus > CargoTokenShipmentCurrentStatus._1) ? btnCreateBookingRequest.Visibility = Visibility.Collapsed
                                                                                           : btnCreateBookingRequest.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCreateBookingRequest.Visibility = Visibility.Collapsed;
                }


            }

            if (Shipment.BookingRequestTokenInfo != null)
            {
                if (string.IsNullOrEmpty(Shipment.BookingRequestTokenInfo.BusinessMetaData)) return;

                ServiceProxy.BookingRequest bookingRequest = JsonConvert.DeserializeObject<ServiceProxy.BookingRequest>(Shipment.BookingRequestTokenInfo.BusinessMetaData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                this.BookingReuqestDataEntityGrid.DataContext = bookingRequest;

                if (Shipment.Contracter == (cboUser.SelectedItem as User).Address)
                {
                    btnCreateBookingConfirmation.Visibility = Visibility.Visible;
                    _ = (Shipment.CurrentStatus > CargoTokenShipmentCurrentStatus._2) ? btnCreateBookingConfirmation.Visibility = Visibility.Collapsed
                                                                        : btnCreateBookingConfirmation.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCreateBookingConfirmation.Visibility = Visibility.Collapsed;
                }


            }

            if (Shipment.BookedTokenInfo != null)
            {
                if (string.IsNullOrEmpty(Shipment.BookedTokenInfo.BusinessMetaData)) return;
                BookingConfirmationInfo bookingConfirmation = JsonConvert.DeserializeObject<BookingConfirmationInfo>(Shipment.BookedTokenInfo.BusinessMetaData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                this.BookingConfirmationDataEntityGrid.DataContext = bookingConfirmation;
            }
        }

        private async void cboUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await QueryMyShipment();

            _ = (((User)cboUser.SelectedItem).Role != Role.Carrier) ? btnCreateQuoteRequest.Visibility = Visibility.Visible : btnCreateQuoteRequest.Visibility = Visibility.Collapsed;
        }

        private async Task QueryMyShipment()
        {
            using (new WaitCursor(this.pgbProgress))
            {
                await GetMyShipment();
            }
        }

        private async void ValidateBusinessEntityButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = false;

            using (new WaitCursor(this.pgbProgress))
            {
                isValid = await ValidateBusinessEntity();
            }

            _ = isValid
                ? MessageBox.Show(this, "Validated!", "ABT Sample App", MessageBoxButton.OK, MessageBoxImage.Information)
                : MessageBox.Show(this, "Invalid Business Entity\n Something seems to be wrong....", "ABT Sample App", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private async Task<bool> ValidateBusinessEntity()
        {
            CargoTokenShipment cargoTokenShipment = this.DataContext as CargoTokenShipment;
            HttpServiceProxy proxy = HttpServiceLocator.GetHttpServiceProxy();
            TokenMintedInfo mintedToken = null;

            switch (tabProcess.SelectedIndex)
            {
                case 0: //QuoteRequest
                    mintedToken = await proxy.GetQuotationRequestTokenAsync(cargoTokenShipment.Id.ToString(), new GetTokenMetaDataRequest()
                    {
                        CallerID = cargoTokenShipment.Contracter,
                        TokenID = cargoTokenShipment.TokenId,
                        TokenSequence = tabProcess.SelectedIndex.ToString()
                    });

                    return cargoTokenShipment.QuoteRequestTokenInfo.BusinessMetaData.Equals(mintedToken.BusinessMetaData) ? true : false;

                case 1: //Quoted
                    mintedToken = await proxy.GetQuotationTokenAsync(cargoTokenShipment.Id.ToString(), new GetTokenMetaDataRequest()
                    {
                        CallerID = cargoTokenShipment.Contracter,
                        TokenID = cargoTokenShipment.TokenId,
                        TokenSequence = tabProcess.SelectedIndex.ToString()
                    });

                    return cargoTokenShipment.QuotedTokenInfo.BusinessMetaData.Equals(mintedToken.BusinessMetaData) ? true : false;

                case 2: //BookingRequest
                    mintedToken = await proxy.GetBookingRequestTokenAsync(cargoTokenShipment.Id.ToString(), new GetTokenMetaDataRequest()
                    {
                        CallerID = cargoTokenShipment.Contracter,
                        TokenID = cargoTokenShipment.TokenId,
                        TokenSequence = tabProcess.SelectedIndex.ToString()
                    });

                    return cargoTokenShipment.BookingRequestTokenInfo.BusinessMetaData.Equals(mintedToken.BusinessMetaData) ? true : false;

                case 3: //BookingConfirmed
                    mintedToken = await proxy.GetBookingConfirmationTokenAsync(cargoTokenShipment.Id.ToString(), new GetTokenMetaDataRequest()
                    {
                        CallerID = cargoTokenShipment.Contracter,
                        TokenID = cargoTokenShipment.TokenId,
                        TokenSequence = tabProcess.SelectedIndex.ToString()
                    });

                    return cargoTokenShipment.BookedTokenInfo.BusinessMetaData.Equals(mintedToken.BusinessMetaData) ? true : false;
                default:
                    return false;
            }
        }

        private void cboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstShipments.ItemsSource == null) return;

            CollectionView view = ((CollectionView)CollectionViewSource.GetDefaultView(lstShipments.ItemsSource));
            view.Filter = UserFilter;

            if (view.Count > 0) lstShipments.SelectedIndex = 0;
        }

        private bool UserFilter(object item)
        {
            if (cboFilter.SelectedIndex == 0)
            {
                return true;
            }
            else
            {
                return ((int)(item as CargoTokenShipment).CurrentStatus == cboFilter.SelectedIndex - 1) ? true : false;
            }
        }

        private void ShowCreateQuoteReuqestWindow()
        {
            Quotation frmQuotation = new Quotation(this, (cboUser.SelectedItem as User).Address, true);
            frmQuotation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            frmQuotation.Owner = this;
            frmQuotation.Show();
        }

        private void ShowCreateBookingRequestWindow()
        {
            BookingRequest frmBookingRequest = new BookingRequest(this, (CargoTokenShipment)this.DataContext);
            frmBookingRequest.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            frmBookingRequest.Owner = this;
            frmBookingRequest.Show();
        }

        private void ShowCreateQuotationWindow()
        {
            Quotation frmQuotation = new Quotation(this, (cboUser.SelectedItem as User).Address);
            frmQuotation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            frmQuotation.Owner = this;
            frmQuotation.Show();
        }

        private void ShowCreateBookingConfirmationWindow()
        {
            BookingConfirmation frmBookingConfirmation = new BookingConfirmation(this, (CargoTokenShipment)this.DataContext);
            frmBookingConfirmation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            frmBookingConfirmation.Owner = this;
            frmBookingConfirmation.Show();
        }


        private void btnCreateBookingRequest_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateBookingRequestWindow();
        }

        private void btnCreateQuotation_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateQuotationWindow();
        }

        private void btnCreateBookingConfirmation_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateBookingConfirmationWindow();
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await QueryMyShipment();
        }

        private void btnCreateQuoteRequest_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateQuoteReuqestWindow();
        }
    }





}
