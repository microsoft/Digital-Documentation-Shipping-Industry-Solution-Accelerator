using ContosoCargo.DigitalDocument.TokenService.OffChain.Mongo.ModelBase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Cache;
using System.Text;

namespace ContosoCargo.DigitalDocument.TokenService.Models
{
    public class PromiseToken : IEntityModel<Guid>
    {
        public PromiseToken()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string ReferenceNo { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }
        public string ContainerSizeType { get; set; }
        public double Rate { get; set; }
        public string Currency { get; set; }
        public string SVVD { get; set; }
        public DateTime ValidUntil { get; set; }
        public int MaximumContainers { get; set; }
        public string Issuer { get; set; }
        public string Mintee { get; set; }
        public int CustomerLevel { get; set; }
    }
}
