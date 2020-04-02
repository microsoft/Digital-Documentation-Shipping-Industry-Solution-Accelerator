using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class TokenMeta
    {
        public string TokenTemplateID { get; set; }
        public string TokenID { get; set; }
        public string TokenName { get; set; }
        public string TokenSymbol { get; set; }
        public string TokenCreator { get; set; }
        public DateTime TokenCreatedDate { get; set; }
    }
}
