using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TokenClientSample.Model;

namespace TokenClientSample
{
    public class PromisetoBook
    {
        const string Carrier = "4419acb3-774b-4ced-ac37-8ff27b783cdc";
        const string Customer = "361b5333-d3df-4480-8f6f-a329a4b7b288";
        const string Shipper = "67e76fc2-c4d7-49cc-8de7-5b29b6b562bc";


        private string PromiseTokenID;
        private TokenClientSample.nFmbtgToken.nFmbtgTokenClient tokenAPI;

        public PromisetoBook(string serviceEndpointI)
        {
            this.tokenAPI = new nFmbtgToken.nFmbtgTokenClient(serviceEndpointI);
        }

        private static long CalulateTokenSequenceNumber(Guid UserAccount)
        {
            return Int64.Parse(UserAccount.ToString().Split("-")[4], System.Globalization.NumberStyles.HexNumber);
        }

        private async Task<string> CreatePromiseToken(string TokenName, string TokenSymbol, string Caller)
        {
            
            var result = await this.tokenAPI.CreateToken(TokenName, TokenSymbol, Caller);
            return result.ContractAddress; //Token ID
        }

        public async void StartProcess()
        {
            //Step 0: Create PromiseToken
            //Create Promise Token
            Console.WriteLine("==============================================================");
            Console.WriteLine("== Step 0. Customer define(create) Promise Token            ==");
            Console.WriteLine("==============================================================");
            var tokenCreationResult = await CreatePromiseToken("ContosoCargo Promise Token", "PM", Carrier);
            PromiseTokenID = tokenCreationResult;
            Console.WriteLine("Creating Promise token....");


            //Step 1: Customer wants get Promise token from Carrier
            //Create Promise Condition
            Console.WriteLine("==============================================================");
            Console.WriteLine("== Step 1. Customer wants to get Promise Token from Carrier ==");
            Console.WriteLine("==============================================================");
            PromiseToken promiseToken = new PromiseToken()
            {
                ReferenceNo = Guid.NewGuid().ToString(),
                Issuer = Carrier,
                Mintee = Customer,
                From = "HongKong",
                Destination = "Vancouver",
                SVVD = "PNW1-OVN-112 E​",
                ValidUntil = new DateTime(2019, 12, 10),
                ContainerSizeType = "20GP",
                MaximumContainers = 10,
                Rate = 1000, //Need be encrypted with Customer's public key
                Currency = "USD"
            };

            var mintResult = await this.tokenAPI.MintToken(PromiseTokenID,
                Carrier,
                Customer,
                JsonConvert.SerializeObject(promiseToken),
                CalulateTokenSequenceNumber(Guid.Parse(Customer)));


            //Check the token my Customer
            var tokenAmount = await this.tokenAPI.GetBalance(PromiseTokenID, Customer);
            if (tokenAmount > 0)
            {
                var promise = await this.tokenAPI.GetTokenMetaData(PromiseTokenID,
                    Customer,
                    CalulateTokenSequenceNumber(Guid.Parse(Customer)));
                Console.WriteLine("I've got Promise Token!");
                Console.WriteLine($"Here is Promise Information\n{promise}");
            }
            else
            {
                Console.WriteLine("make wait time bit more longer");
                return;
            }

            await CheckWallet();

            //Step 2: Customer transfer Promise Token to Shipper
            Console.WriteLine("\n\n\n================================================================");
            Console.WriteLine("== Step 2. Customer want to transfer promise token to Shipper ==");
            Console.WriteLine("================================================================");
            var transferResult = await this.tokenAPI.TranferToken(PromiseTokenID,
                 CalulateTokenSequenceNumber(Guid.Parse(Customer)),
                 Customer, Customer, Shipper);

            //Check the token my Customer
            tokenAmount = await this.tokenAPI.GetBalance(PromiseTokenID, Shipper);
            if (tokenAmount > 0)
            {
                var promise = await this.tokenAPI.GetTokenMetaData(PromiseTokenID,
                    Shipper,
                    CalulateTokenSequenceNumber(Guid.Parse(Customer)));
                Console.WriteLine("Customer Sent Promise Token to make him book with Promise Information");
                Console.WriteLine($"Here is Promise Information from transfered Promise token \n{promise}");
            }
            else
            {
                Console.WriteLine("make wait time bit more longer");
                return;
            }

            await CheckWallet();

            //Step 3: Shipper want to book with Promise Information
            //Assume Shipper try to book with Promise Information after transfering Promise token to Carrier
            Console.WriteLine("\n\n\n=====================================================================================================");
            Console.WriteLine("== Step 3. Shipper try to book with Promise Information after transfering Promise token to Carrier ==");
            Console.WriteLine("=====================================================================================================");

            transferResult = await this.tokenAPI.TranferToken(PromiseTokenID,
                          CalulateTokenSequenceNumber(Guid.Parse(Customer)),
                          Shipper, Shipper, Carrier);

            

            //Check the token my Customer
            tokenAmount = await this.tokenAPI.GetBalance(PromiseTokenID, Carrier);
            if (tokenAmount > 0)
            {
                var promise = await this.tokenAPI.GetTokenMetaData(PromiseTokenID,
                    Carrier,
                    CalulateTokenSequenceNumber(Guid.Parse(Customer)));
                Console.WriteLine("Shipper has transferred Promise token to Carrier for booking with Promise Information");
                Console.WriteLine($"Here is Promise Information\n{promise}");
            }
            else
            {
                Console.WriteLine("make wait time bit more longer");
                return;
            }

            await CheckWallet();

            //Step 4: Carrier claim shipping cost based on PromiseToken
            Console.WriteLine("\n\n\n======================================================================");
            Console.WriteLine("== Step 4. Carrier can check Promise Information from transferred token ==");
            Console.WriteLine("======================================================================");
            Console.WriteLine("Carrier check reference number and validate Promise Information");

            //Check shipping cost from Promise Token
            var promiseInfo = await this.tokenAPI.GetTokenMetaData(PromiseTokenID,
                   Carrier,
                   CalulateTokenSequenceNumber(Guid.Parse(Customer)));

            var promiseObj = JsonConvert.DeserializeObject<PromiseToken>(promiseInfo);

            Console.WriteLine($"Checked Reference number and it's Owner - {promiseObj.Rate} {promiseObj.Currency}, The reference Number is : {promiseObj.ReferenceNo} and Promise Token owner is {promiseObj.Mintee}");



            //Step 6: After payment The token will be burnt
            Console.WriteLine("\n\n\n==================================================================================================");
            Console.WriteLine("== Step 5. Carrier pass Booking information to their internal ERP system for book the space of vessel ==");
            Console.WriteLine("========================================================================================================");
            Console.WriteLine("OK... we processed for booking based on your booking information and burn this token");

            var result = await this.tokenAPI.BurnToken(PromiseTokenID,
                Carrier,
                CalulateTokenSequenceNumber(Guid.Parse(Customer)));

        

            await CheckWallet();


            //Step 5: Carrier sending Booking Confirmation to Shipper
            Console.WriteLine("\n\n\n=========================================================");
            Console.WriteLine("== Step 6. Carrier send back Booking Confirmation to shipper ==");
            Console.WriteLine("===============================================================");

            Console.WriteLine("Carrier sent Booking Confirmation to Shipper.......");

            Console.WriteLine("\n\nAll Process done...");
        }

        private async Task CheckWallet()
        {
            //Check the wallets
            var carrierBalance = await this.tokenAPI.GetBalance(PromiseTokenID, Carrier);
            var customerBalance = await this.tokenAPI.GetBalance(PromiseTokenID, Customer);
            var shipperBalance = await this.tokenAPI.GetBalance(PromiseTokenID, Shipper);

            Console.WriteLine($"Carrier Wallet has {carrierBalance} tokens");
            Console.WriteLine($"Customer Wallet has {customerBalance} tokens");
            Console.WriteLine($"Shipper Wallet has {shipperBalance} tokens");
        }
    }
}
