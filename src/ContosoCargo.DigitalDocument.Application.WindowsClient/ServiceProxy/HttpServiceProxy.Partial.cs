using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoSmart.Windows.Booking.ServiceProxy
{
    public partial class HttpServiceProxy
    {
        public string HostKey { get; set; }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            if (url.Contains("localhost")) return;

            url += $"?code={HostKey}";
            request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);
        }
    }
}
