using System;
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
                var pSlackChannelName = new SqlParameter
                {
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.NVarChar,
                    ParameterName = "@SlackChannelName",
                    Value = slackChannelInfo.SlackChannelName
                };
                var pPivotalProjectId = new SqlParameter
                {
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.NVarChar,
                    ParameterName = "@PivotalProjectId",
                    Value = slackChannelInfo.PivotalProjectId
                };

                using (var command = new SqlCommand("Update_SlackChannel", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Parameters = {pSlackChannelName, pPivotalProjectId}
                })
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

        }

        public SlackChannelInfo GetSlackChannelInfo(string slackChannelName)
        {
            throw new NotImplementedException();
        }
    }
}
