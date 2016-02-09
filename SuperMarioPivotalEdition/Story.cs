using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioPivotalEdition
{

    public class Story
    {
        public string kind { get; set; }
        public int id { get; set; }
        public int project_id { get; set; }
        public string name { get; set; }
        public string story_type { get; set; }
        public string current_state { get; set; }
        public int estimate { get; set; }
        public int requested_by_id { get; set; }
        public object[] owner_ids { get; set; }
        public object[] labels { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
    }
}
