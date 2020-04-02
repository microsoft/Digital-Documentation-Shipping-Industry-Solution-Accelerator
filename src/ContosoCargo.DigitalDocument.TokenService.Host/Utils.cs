using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ContosoCargo.DigitalDocument.TokenService.Host
{
    public static class Utils
    {
        public static void WaitSeconds(int seconds)
        {
            var destinationTime = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Now < destinationTime)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
