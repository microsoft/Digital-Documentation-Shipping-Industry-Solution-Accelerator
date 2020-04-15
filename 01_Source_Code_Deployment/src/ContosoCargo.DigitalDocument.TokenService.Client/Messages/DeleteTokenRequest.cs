namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class DeleteTokenRequest
    {
        public string TokenID { get; set; }
        public string CallerID { get; set; }
        public long? TokenSequence { get; set; }
    }
}
