// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using ContosoCargo.DigitalDocument.TokenService.Models;
using ContosoCargo.DigitalDocument.TokenService.Host.Messages;
using ContosoCargo.DigitalDocument.TokenService.Host.Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.OpenApi.Models;

namespace ContosoCargo.DigitalDocument.TokenService.Host
{
    public class ContosoCargo
    {
        private readonly IContosoCargoApplication _cargoApp;
        private ILogger<ContosoCargo> log;
        public ContosoCargo(IContosoCargoApplication CargoApp, ILogger<ContosoCargo> Logger)
        {
            (_cargoApp, log) = (CargoApp, Logger);
        }


        [Function(nameof(CreateQuoteRequest))]
        [OpenApiOperation(operationId: "CreateQuoteRequest",
            Description = "Create Shipment and then mint Quotation Token",
            Summary = "Create Shipment and mint Quotation Token")]
        [OpenApiRequestBody("application/json", typeof(QuoteRequestMessage), Description = "QuotationRequest Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CargoTokenShipment), Description = "Whole Shipment Document")]
        public async Task<IActionResult> CreateQuoteRequest(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Shipment/QuoteRequest")] HttpRequestData req)
        {
            log.LogInformation("C# HTTP trigger function processed a request. - CargoSmart/QuoteRequest");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QuoteRequestMessage data = JsonConvert.DeserializeObject<QuoteRequestMessage>(requestBody);

            var result = await _cargoApp.CreateQuoteRequest(data);

            return new OkObjectResult(result);
        }


        [Function(nameof(CreateQuote))]
        [OpenApiOperation("CreateQuote",
            Description = "Mining Quotation Token",
            Summary = "when Carrier agreed for Quotation Request, Quotation Token will be minted")]
        [OpenApiRequestBody("application/json", typeof(QuoteRequestMessage), Description = "QuotationRequest Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CargoTokenShipment), Description = "Whole Shipment Document")]
        public async Task<IActionResult> CreateQuote(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Shipment/Quote")] HttpRequestData req)
        {
            log.LogInformation("C# HTTP trigger function processed a request. - CargoSmart/Quote");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QuoteRequestMessage data = JsonConvert.DeserializeObject<QuoteRequestMessage>(requestBody);

            var result = await _cargoApp.CreateQuote(data);

            return new OkObjectResult(result);
        }



        [Function(nameof(CreateBookingRequest))]
        [OpenApiOperation("CreateBookingRequest",
            Description = "Minting BookingRequest token",
            Summary = "when Customer agreed for Quotation, BookingRequest information is updated in Shipment and BookingRequest token is minted to customer.")]
        [OpenApiParameter("id", In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(BookingRequestMessage), Description = "BookingRequest Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CargoTokenShipment), Description = "Whole Shipment Document")]
        public async Task<IActionResult> CreateBookingRequest(
    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Shipment/{id}/Booking")] HttpRequestData req)
        {
            log.LogInformation("C# HTTP trigger function processed a request. - put CargoSmart/Booking");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BookingRequestMessage data = JsonConvert.DeserializeObject<BookingRequestMessage>(requestBody);

            var result = await _cargoApp.BookingRequest(data);

            return new OkObjectResult(result);
        }

        [Function(nameof(CreateBookingConfirmation))]
        [OpenApiOperation("CreateBookingConfirmation",
            Description = "Minting BookingConfirmation token",
            Summary = "when booking request has been confirmed from customer side, BookingConfirmation information is updated in offchaindatbase and BookingConfirmation token is minted.")]
        [OpenApiParameter("id", In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(BookingConfirmationRequestMessage), Description = "BookingConfirmation Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CargoTokenShipment), Description = "Whole Shipment Document")]
        public async Task<IActionResult> CreateBookingConfirmation(
[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Shipment/{id}/BookingConfirmation")] HttpRequestData req)
        {
            log.LogInformation("C# HTTP trigger function processed a request. - CargoSmart/BookingConfirmation");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BookingConfirmationRequestMessage data = JsonConvert.DeserializeObject<BookingConfirmationRequestMessage>(requestBody);

            var result = await _cargoApp.BookingConfimation(data);

            return new OkObjectResult(result);
        }

        [Function(nameof(GetMyShipments))]
        [OpenApiOperation("GetMyShipments",
            Description = "Get Shipments what I created or has got from",
            Summary = "list up whole Shipments which were created by me or minted token to me")]
        [OpenApiRequestBody("application/json", typeof(GetTokenShipmentsRequest), Description = "TokenIncidnentRequest Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(IEnumerable<CargoTokenShipment>), Description = "Get Shipment Document lists")]
        public IEnumerable<CargoTokenShipment> GetMyShipments(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Shipment")] HttpRequestData req)
        {
            log.LogInformation("C# HTTP trigger function processed a request. - post CargoSmart/Shipment");

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            GetTokenShipmentsRequest data = JsonConvert.DeserializeObject<GetTokenShipmentsRequest>(requestBody);

            IEnumerable<CargoTokenShipment> result = _cargoApp.GetMyShipments(data);

            return result;
        }

        [Function(nameof(DeleteMyShipment))]
        [OpenApiOperation("DeleteMyShipment",
            Description = "delete Shipment that I created",
            Summary = "Delete Shipment record and burn tokens from offchain and chain both")]
        [OpenApiParameter("id", Type = typeof(Guid) ,In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(DeleteTokenRequest), Description = "DeleteToken request Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(System.Boolean), Description = "Delete process result")]
        public async Task<IActionResult> DeleteMyShipment(
  [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Shipment/{id}")] HttpRequestData req, Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. - delete CargoSmart/Shipment/{id}");


            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            DeleteTokenRequest data = JsonConvert.DeserializeObject<DeleteTokenRequest>(requestBody);

            var result = await _cargoApp.DeleteMyShipment(id, data);

            return new OkObjectResult(result);
        }

        [Function(nameof(GetShipment))]
        [OpenApiOperation("GetShipment",
            Description = "get specific Shipment data by Shipment ID",
            Summary = "Get Shipment Data by Shipment ID")]
        [OpenApiParameter("id", Type = typeof(Guid), In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CargoTokenShipment), Description = "Whole Shipment Document")]
        public CargoTokenShipment GetShipment(
              [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Shipment/{id}")] HttpRequestData req,
              Guid id)
        {

            log.LogInformation($"C# HTTP trigger function processed a request. - get CargoSmart/Shipment/{id}");

            var result = _cargoApp.GetShipment(id);

            return result;
        }

        [Function("GetQuotationRequestToken")]
        [OpenApiOperation("GetQuotationRequestToken",
            Description = "Return QuotationRequest Token Information by Shipment ID",
            Summary = "Get QuotationRequest Token by Shipment ID")]
        [OpenApiParameter("id", Type = typeof(Guid), In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(GetTokenMetaDataRequest), Description = "QuotationRequest token request Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(TokenMintedInfo), Description = "QuoteRequest Token Information")]
        public async Task<TokenMintedInfo> GetQuoteRequestTokenFromChain(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Shipment/{id}/QuoteRequestToken")] HttpRequestData req, Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. - get GetQuoteTokenFromChain CargoSmart/Shipment/{id}");

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            GetTokenMetaDataRequest data = JsonConvert.DeserializeObject<GetTokenMetaDataRequest>(requestBody);

            var result = await _cargoApp.GetMintedTokenInfoFromChain<Quotation>(id, data);

            return result;
        }

        [Function("GetQuotationToken")]
        [OpenApiOperation("GetQuotationToken",
            Description = "Return Quotation Token Information by Shipment ID",
            Summary = "Get Quotation Token by Shipment ID")]
        [OpenApiParameter("id", Type = typeof(Guid), In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(GetTokenMetaDataRequest), Description = "Quotation token request Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(TokenMintedInfo), Description = "Quote Token Information")]
        public async Task<TokenMintedInfo> GetQuoteTokenFromChain(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Shipment/{id}/QuoteToken")] HttpRequestData req, Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. - get GetQuoteTokenFromChain CargoSmart/Shipment/{id}");

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            GetTokenMetaDataRequest data = JsonConvert.DeserializeObject<GetTokenMetaDataRequest>(requestBody);

            var result = await _cargoApp.GetMintedTokenInfoFromChain<RateQuotation>(id, data);

            return result;
        }

        [Function("GetBookingRequestToken")]
        [OpenApiOperation("GetBookingRequestToken",
            Description = "Return BookingRequest Token Information by Shipment ID",
            Summary = "Get BookingRequest Token by Shipment ID")]
        [OpenApiParameter("id", Type = typeof(Guid), In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(GetTokenMetaDataRequest), Description = "BookingRequest token request Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(TokenMintedInfo), Description = "BookingRequest Token Information")]
        public async Task<TokenMintedInfo> GetBookingRequestTokenFromChain(
[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Shipment/{id}/BookingToken")] HttpRequestData req, Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. - get GetBookingRequestToken CargoSmart/Shipment/{id}");

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            GetTokenMetaDataRequest data = JsonConvert.DeserializeObject<GetTokenMetaDataRequest>(requestBody);

            var result = await _cargoApp.GetMintedTokenInfoFromChain<BookingRequest>(id, data);

            return result;
        }

        [Function("GetBookingConfirmationToken")]
        [OpenApiOperation("GetBookingConfirmationToken",
            Description = "Return BookingConfirmation Token Information by Shipment ID",
            Summary = "Get BookingConfirmation Token by Shipment ID")]
        [OpenApiParameter("id", Type = typeof(Guid), In = Microsoft.OpenApi.Models.ParameterLocation.Path, Description = "Shipment ID", Required = true)]
        [OpenApiRequestBody("application/json", typeof(GetTokenMetaDataRequest), Description = "BookingConfirmation token request Information")]
        [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(TokenMintedInfo), Description = "BookingConfirmation Token Information")]
        public async Task<TokenMintedInfo> GetBookingConfirmationTokenFromChain(
[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Shipment/{id}/BookingConfirmationToken")] HttpRequestData req, Guid id)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. - get GetBookingConfirmationToken CargoSmart/Shipment/{id}");

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            GetTokenMetaDataRequest data = JsonConvert.DeserializeObject<GetTokenMetaDataRequest>(requestBody);

            var result = await _cargoApp.GetMintedTokenInfoFromChain<BookingConfirmation>(id, data);

            return result;
        }
    }
}
