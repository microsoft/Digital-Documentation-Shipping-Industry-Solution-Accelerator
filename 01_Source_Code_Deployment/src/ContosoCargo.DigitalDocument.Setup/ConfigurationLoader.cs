using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ContosoCargo.DigitalDocument.Setup
{
    public class ConfigurationLoader
    {
        private static IConfigurationRoot configuration;

        public static IConfigurationRoot Config { get
            {
                if (configuration == null)
                {
                    configuration = loadConfigFile();
                }

                return configuration;
            } 
        }

        private static IConfigurationRoot loadConfigFile()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(Path.Combine(fileInfo.Directory.FullName, "appsettings.json")))
            {
                throw new FileNotFoundException("there is no appsettings.json file");
            }

            return new ConfigurationBuilder()
                                               .SetBasePath(fileInfo.Directory.FullName)
                                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                               .Build();

            
        }
    }
}
