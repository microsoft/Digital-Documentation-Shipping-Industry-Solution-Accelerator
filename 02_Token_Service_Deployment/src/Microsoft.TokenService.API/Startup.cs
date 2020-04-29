using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.TokenService.BlockchainNetworkManager;
using Microsoft.TokenService.LedgerClient.Client;
using Microsoft.TokenService.PartyManager;
using Microsoft.TokenService.UserManager;

namespace Microsoft.TokenService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microsoft Token Service API Endpoint", Version = "v1.0.0" });
                c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
            });

            services.AddSwaggerGenNewtonsoftSupport();

            //add DI
            services.AddTransient<IBlockchainNetworkManager, BlockchainNetworks>(c =>
            {
                return new BlockchainNetworks(Configuration["App:Offchain_Connectionstring"],
                                                Configuration["App:ManagementCollection"]);
            });

            services.AddTransient<IPartyManager, Parties>(c =>
            {
                return new Parties(Configuration["App:Offchain_Connectionstring"],
                                                Configuration["App:ManagementCollection"]);
            });

            services.AddTransient<IUserManager, Users>(c =>
            {
                return new Users(Configuration["App:Offchain_Connectionstring"],
                                                Configuration["App:ManagementCollection"], Configuration);
            });

            services.AddTransient<InFmbtgTokenClient, nFmbtgTokenClient>(c =>
            {
                return new nFmbtgTokenClient(Configuration);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }



            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwagger(c =>
            {
                c.RouteTemplate =
                    "api-docs/{documentName}/swagger.json";

            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microsoft Token Service API V1");

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
