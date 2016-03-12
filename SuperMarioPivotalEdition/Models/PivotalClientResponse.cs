using System.Net.Http;

namespace SuperMarioPivotalEdition.Models
{
    class PivotalClientResponse
    {
        public string ShortResponseMessage { get; set; } = "";
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
