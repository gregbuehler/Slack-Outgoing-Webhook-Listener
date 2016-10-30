namespace ApiIntegrations.Models.GoogleVision
{
    public class GoogleVisionResponse
    {
        public Respons[] responses { get; set; }
    }

    public class Respons
    {
        public Labelannotation[] labelAnnotations { get; set; }
    }

    public class Labelannotation
    {
        public string mid { get; set; }
        public string description { get; set; }
        public float score { get; set; }
    }
}