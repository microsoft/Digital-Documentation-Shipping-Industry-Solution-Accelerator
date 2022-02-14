// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Configuration;

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
