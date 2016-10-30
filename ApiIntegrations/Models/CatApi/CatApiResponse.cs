using System.Xml.Serialization;

namespace ApiIntegrations.Models.CatApi
{
    /// <remarks />
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CatApiResponse
    {
        /// <remarks />
        public responseData data { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class responseData
    {
        /// <remarks />
        [XmlArrayItem("image", IsNullable = false)]
        public responseDataImage[] images { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class responseDataImage
    {
        /// <remarks />
        public string url { get; set; }

        /// <remarks />
        public string id { get; set; }

        /// <remarks />
        public string source_url { get; set; }
    }
}