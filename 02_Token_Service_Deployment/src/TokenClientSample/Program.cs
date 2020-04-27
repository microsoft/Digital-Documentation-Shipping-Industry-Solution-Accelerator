using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace TokenClientSample
{
    class Program
    {
        static IConfigurationRoot config;
        
        static void Main(string[] args)
        {
            config = ReadSettings();
            PromisetoBook testObj = new PromisetoBook(config["App:EndpointURL"]);
            testObj.StartProcess();
            Console.ReadLine();
        }





        private static IConfigurationRoot ReadSettings()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = null;


            if (File.Exists(Path.Combine(fileInfo.Directory.FullName, "application.settings.json")))
            {
                //Normal Application
                path = fileInfo.Directory.FullName;
            }
            else if (File.Exists(Path.Combine(fileInfo.Directory.Parent.FullName, "application.settings.json")))
            {
                //For Function App
                path = fileInfo.Directory.Parent.FullName;
            }
            else if (File.Exists(Path.Combine(fileInfo.Directory.FullName, "appsettings.json")))
            {
                //Normal Application
                path = fileInfo.Directory.FullName;
                //For ASP.net core
                return new ConfigurationBuilder()
                       .SetBasePath(path)
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .Build();
            }

            return new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("application.settings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }


}
