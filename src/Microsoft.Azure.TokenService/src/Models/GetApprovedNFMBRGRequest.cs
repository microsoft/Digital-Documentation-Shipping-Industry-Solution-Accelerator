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
    /// Request for the getApproved operation
    /// </summary>
    public partial class GetApprovedNFMBRGRequest
    {
        /// <summary>
        /// Initializes a new instance of the GetApprovedNFMBRGRequest class.
        /// </summary>
        public GetApprovedNFMBRGRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetApprovedNFMBRGRequest class.
        /// </summary>
        public GetApprovedNFMBRGRequest(string callerGroupName = default(string), string callerAccountName = default(string), string requestId = default(string), GetApprovedNFMBRGRequestFunctionParams functionParams = default(GetApprovedNFMBRGRequestFunctionParams))
        {
            CallerGroupName = callerGroupName;
            CallerAccountName = callerAccountName;
            RequestId = requestId;
            FunctionParams = functionParams;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CallerGroupName")]
        public string CallerGroupName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CallerAccountName")]
        public string CallerAccountName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RequestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FunctionParams")]
        public GetApprovedNFMBRGRequestFunctionParams FunctionParams { get; set; }

    }
}
