// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.TokenService.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Response from the transferFromNFMBRG operation
    /// </summary>
    public partial class TransferFromNFMBRGResponse
    {
        /// <summary>
        /// Initializes a new instance of the TransferFromNFMBRGResponse class.
        /// </summary>
        public TransferFromNFMBRGResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TransferFromNFMBRGResponse class.
        /// </summary>
        public TransferFromNFMBRGResponse(bool? result = default(bool?))
        {
            Result = result;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public bool? Result { get; set; }

    }
}
