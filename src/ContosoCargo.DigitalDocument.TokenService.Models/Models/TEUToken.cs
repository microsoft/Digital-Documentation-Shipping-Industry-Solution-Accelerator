using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class TEUToken
    {
        public string From { get; set; }
        public string To { get; set; }
        public string ContainerSizeType { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Issuer { get; set; }
    }
}
