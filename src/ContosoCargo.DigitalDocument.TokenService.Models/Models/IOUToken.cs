using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class IOUToken
    {
        public string Currency { get; set; }
        public string Issuer { get; set; }
        public string Creditor { get; set; }
        public int TEUTokenAmount { get; set; }
        public double TokenPrice { get; set; }
        public DateTime OwedDate { get; set; }
    }
}
