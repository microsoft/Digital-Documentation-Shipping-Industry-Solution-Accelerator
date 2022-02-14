using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ContosoCargo.DigitalDocument.TokenService.Host.Application;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Polly;
using Polly.Extensions.Http;


namespace ContosoCargo.DigitalDocument.TokenService.Host
{
    public class Program 
    {
        public async static Task Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                .ConfigureAppConfiguration(appConfiguration =>
                {
                    var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
                    appConfiguration.AddJsonFile(Path.Combine(fileInfo.Directory.FullName,"application.settings.json"), false, false).Build();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<CosmosConnectionStrings>(x =>
                    {
                        return ConnectionStringAccessor.Create(context.Configuration["App:SubscriptionId"], context.Configuration["App:ResourceGroupName"], context.Configuration["App:DatabaseAccountName"])
                            .GetConnectionStringsAsync(context.Configuration["App:ManagedIdentityId"]).GetAwaiter().GetResult();
                    });

                    services.AddHttpClient<IContosoCargoApplication, Application.ContosoCargo>()
                                 .SetHandlerLifetime(TimeSpan.FromSeconds(5))
                                 .AddPolicyHandler(GetRetryPolicy());

                    services.AddTransient<IContosoCargoApplication, Application.ContosoCargo>();
                    services.AddLogging();
                })
                .Build();

            await host.RunAsync();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }

    }
}
