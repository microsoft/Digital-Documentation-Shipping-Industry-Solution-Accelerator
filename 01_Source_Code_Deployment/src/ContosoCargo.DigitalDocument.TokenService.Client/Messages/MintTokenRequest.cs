namespace ContosoCargo.DigitalDocument.TokenService.Client
{
    public class MintTokenRequest
    {
        public string TokenID { get; set; }
        public string Minter { get; set; }
        public string Mintee { get; set; }
        public string SerializedBusinessMetaData { get; set; }
        public string TokenSequence { get; set; }

    }
}
