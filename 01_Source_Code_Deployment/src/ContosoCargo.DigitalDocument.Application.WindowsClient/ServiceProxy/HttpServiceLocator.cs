using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoSmart.Windows.Booking.ServiceProxy
{
    public class HttpServiceLocator
    {
        private static string ReadValue(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }

        public static HttpServiceProxy GetHttpServiceProxy()
        {
           return new HttpServiceProxy(new System.Net.Http.HttpClient()) { 
               BaseUrl = HttpServiceLocator.ReadValue("ServiceEndPoint"),
               HostKey = HttpServiceLocator.ReadValue("HostKey")
           };
        }
        
    }
}
