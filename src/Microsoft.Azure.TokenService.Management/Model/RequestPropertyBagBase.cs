using Microsoft.Azure.TokenService.Management.Interfaces;
using System.Collections.Generic;

namespace Microsoft.Azure.TokenService.Management.Model
{
    public class RequestPropertyBagBase : IRequestPropertyBag
    {
        public string description { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var dicProperties = new Dictionary<string, object>();

            foreach (var item in properties)
            {
                if (item.GetCustomAttributes(typeof(IgnoreToList), true).Length == 0)
                    dicProperties.Add(item.Name, item.GetValue(this));
            }

            return dicProperties;
        }
    }
}
