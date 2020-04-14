namespace Microsoft.Azure.TokenService.Management.Model
{
    public class GroupRequestPropertyBag : RequestPropertyBagBase
    {
        [IgnoreToList]
        public string GroupName { get; set; }
    }
}
