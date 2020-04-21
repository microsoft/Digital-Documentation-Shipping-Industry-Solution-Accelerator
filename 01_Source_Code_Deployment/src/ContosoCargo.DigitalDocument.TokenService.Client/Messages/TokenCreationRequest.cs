namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class TokenCreationRequest
    {
        public string TokenName { get; set; }
        public string TokenSymbol { get; set; }
        public string CallerID { get; set; }
    }
}
