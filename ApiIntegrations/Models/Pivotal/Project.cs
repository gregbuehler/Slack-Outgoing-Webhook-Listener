using System;

namespace ApiIntegrations.Models.Pivotal
{
    public class Project
    {
        public int account_id { get; set; }
        public bool atom_enabled { get; set; }
        public bool automatic_planning { get; set; }
        public bool bugs_and_chores_are_estimatable { get; set; }
        public DateTime created_at { get; set; }
        public int current_iteration_number { get; set; }
        public string description { get; set; }
        public bool enable_following { get; set; }
        public bool enable_incoming_emails { get; set; }
        public bool enable_tasks { get; set; }
        public bool has_google_domain { get; set; }
        public int id { get; set; }
        public int initial_velocity { get; set; }
        public int iteration_length { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public int number_of_done_iterations_to_show { get; set; }
        public string point_scale { get; set; }
        public bool point_scale_is_custom { get; set; }
        public string profile_content { get; set; }
        public string project_type { get; set; }
        public bool _public { get; set; }
        public string start_date { get; set; }
        public DateTime start_time { get; set; }
        public Time_Zone time_zone { get; set; }
        public DateTime updated_at { get; set; }
        public int velocity_averaged_over { get; set; }
        public int version { get; set; }
        public string week_start_day { get; set; }
    }

    public class Time_Zone
    {
        public string kind { get; set; }
        public string olson_name { get; set; }
        public string offset { get; set; }
    }
}