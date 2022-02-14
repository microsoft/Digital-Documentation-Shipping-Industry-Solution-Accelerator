// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContosoCargo.DigitalDocument.Setup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();

        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddHttpClient<ContosoCargoDigitalDocumentSetup>()
                                .SetHandlerLifetime(TimeSpan.FromSeconds(5))
                                .AddPolicyHandler(GetRetryPolicy());

                            services.AddTransient<ContosoCargoDigitalDocumentSetup>();
                            services.AddHostedService<SetupApplication>();
                        }
                        );
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


