namespace SuperMarioPivotalEdition.Models
{

    public class ImgurResponse
    {
        public Datum[] data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public long bandwidth { get; set; }
        public object vote { get; set; }
        public bool favorite { get; set; }
        public bool nsfw { get; set; }
        public string section { get; set; }
        public string account_url { get; set; }
        public int? account_id { get; set; }
        public bool in_gallery { get; set; }
        public string topic { get; set; }
        public int topic_id { get; set; }
        public string link { get; set; }
        public int comment_count { get; set; }
        public int ups { get; set; }
        public int downs { get; set; }
        public int points { get; set; }
        public int score { get; set; }
        public bool is_album { get; set; }
        public string gifv { get; set; }
        public string webm { get; set; }
        public string mp4 { get; set; }
        public string mp4_size { get; set; }
        public string webm_size { get; set; }
        public bool looping { get; set; }
        public string cover { get; set; }
        public int cover_width { get; set; }
        public int cover_height { get; set; }
        public string privacy { get; set; }
        public string layout { get; set; }
        public int images_count { get; set; }
    }
}
