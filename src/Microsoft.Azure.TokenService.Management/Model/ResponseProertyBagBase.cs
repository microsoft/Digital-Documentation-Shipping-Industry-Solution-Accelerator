using Microsoft.Azure.TokenService.Management.Interfaces;

namespace Microsoft.Azure.TokenService.Management.Model
{
    public class ResponseProertyBagBase<T> where T : IResponsePropertyBag
    {
        public ResponseResourceObject<T>[] value { get; set; }
    }

    public class ResponseResourceObject<T>
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public T properties { get; set; }
    }
}
