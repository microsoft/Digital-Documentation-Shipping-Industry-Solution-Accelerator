using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using Aliencube.AzureFunctions.Extensions.OpenApi;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using Aliencube.AzureFunctions.Extensions.OpenApi.Extensions;
using Aliencube.AzureFunctions.Extensions.OpenApi.Configurations;
using System.Reflection;
using Microsoft.OpenApi;
using Newtonsoft.Json.Serialization;

namespace ContosoCargo.DigitalDocument.TokenService.Host
{
    public static class SwaggerRanderer
    {
        [FunctionName(nameof(RenderSwaggerDocument))]
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerDocument(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger.json")] HttpRequest req,
    ILogger log)
        {

            OpenApiInfo apiInfo = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "Swagger Document for CargoSmart API",
                License = new OpenApiLicense() { Name = "Microsoft", Url = new Uri("http://www.microsoft.com") }
            };

            //var settings = new AppSettings();
            var filter = new RouteConstraintFilter();
            var helper = new DocumentHelper(filter);
            var document = new Document(helper);
            var result = await document.InitialiseDocument()
                                       .AddMetadata(apiInfo)
                                       .AddServer(req, "api")
                                       .Build(Assembly.GetExecutingAssembly(), new CamelCaseNamingStrategy())
                                       .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json)
                                       .ConfigureAwait(false);
            var response = new ContentResult()
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = 200
            };

            return response;
        }


        [FunctionName(nameof(RenderSwaggerUI))]
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerUI(
       [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/ui")] HttpRequest req,
          ILogger log)
        {
            //var settings = new Aliencube.AzureFunctions.Extensions.Configuration.AppSettings();
            var ui = new SwaggerUI();

            OpenApiInfo apiInfo = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "Swagger Document for CargoSmart API",
                License = new OpenApiLicense() { Name = "Microsoft", Url = new Uri("http://www.microsoft.com")}
            };

            var result = await ui.AddMetadata(apiInfo)
                                 .AddServer(req, "api")
                                 .BuildAsync()
                                 .RenderAsync("swagger.json", null)
                                 .ConfigureAwait(false);
            var response = new ContentResult()
            {
                Content = result,
                ContentType = "text/html",
                StatusCode = 200
            };

            return response;
        }
    }
}
