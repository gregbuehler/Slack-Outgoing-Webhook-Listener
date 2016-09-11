using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SuperMarioPivotalEdition.Models;

namespace SuperMarioPivotalEdition.Data
{
    class SqlDatabaseClient : IDatabaseClient
    {
        public void UpdateSlackChannelInfo(SlackChannelInfo slackChannelInfo)
        {
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["SqlConnectionString"]))
            {
                connection.Open();
                using (var command = new SqlCommand("Update_SlackChannel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SlackChannelName", slackChannelInfo.SlackChannelName);
                    command.Parameters.AddWithValue("@PivotalProjectId", slackChannelInfo.PivotalProjectId);
                    command.Parameters.AddWithValue("@Descriptions", CreateDataTable(slackChannelInfo.DefaultTaskDescriptions));
                    command.ExecuteNonQuery();
                }
            }
        }

        public SlackChannelInfo GetSlackChannelInfo(string slackChannelName)
        {
            string pivotalId;
            var descriptions = new List<string>();
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["SqlConnectionString"]))
            {
                connection.Open();
                using (var command = new SqlCommand("Get_SlackChannel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SlackChannelName", slackChannelName);
                    var reader = command.ExecuteReader();
                    reader.Read();
                    pivotalId = (string)reader["PivotalProjectId"];
                    reader.Close();
                }
                using (var command = new SqlCommand("Get_DefaultTaskDescription", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SlackChannelName", slackChannelName);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        descriptions.Add((string)reader["Description"]);
                    }
                    reader.Close();
                }
            }
            return new SlackChannelInfo(slackChannelName, pivotalId, descriptions);
        }

        private static DataTable CreateDataTable(IEnumerable<string> ids)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }
    }
}
