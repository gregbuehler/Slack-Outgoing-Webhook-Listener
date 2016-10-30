using System;

namespace ApiIntegrations.Models.Pivotal
{
    public class Story
    {
        public DateTime created_at { get; set; }
        public string current_state { get; set; }
        public string description { get; set; }
        public int estimate { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public Label[] labels { get; set; }
        public string name { get; set; }
        public int[] owner_ids { get; set; }
        public int project_id { get; set; }
        public int requested_by_id { get; set; }
        public string story_type { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public int[] task_ids { get; set; }
        public Task[] tasks { get; set; }
    }

    public class Label
    {
        public string kind { get; set; }
        public int id { get; set; }
        public int project_id { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}