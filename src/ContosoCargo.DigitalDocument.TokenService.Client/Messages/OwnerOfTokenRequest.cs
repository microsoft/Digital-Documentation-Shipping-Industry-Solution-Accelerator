namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class OwnerOfTokenRequest
    {
        public string TokenID { get; set; }
        public string CallerID { get; set; }
        public string TokenSequence { get; set; }
    }
}
