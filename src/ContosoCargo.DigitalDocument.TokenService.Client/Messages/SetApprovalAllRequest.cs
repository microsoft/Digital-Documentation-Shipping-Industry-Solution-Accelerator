namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class SetApprovalAllRequest
    {
        public string TokenID { get; set; }
        public string CallerID { get; set; }
        public string AllowerID { get; set; }
    }
}
