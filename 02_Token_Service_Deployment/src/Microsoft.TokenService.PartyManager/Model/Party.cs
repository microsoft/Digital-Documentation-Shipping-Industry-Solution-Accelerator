using Microsoft.TokenService.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.TokenService.PartyManager.Model
{
    public class Party : IEntityModel<Guid>
    {
        public Guid Id { get ; set ; }

        public Party()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }

        public string PartyName { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
    }
}
