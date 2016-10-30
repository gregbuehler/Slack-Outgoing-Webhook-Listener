using System;

namespace ApiIntegrations.Models.Pivotal
{
    public class Task
    {
        public string kind { get; set; }
        public int id { get; set; }
        public int story_id { get; set; }
        public string description { get; set; }
        public bool complete { get; set; }
        public int position { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}