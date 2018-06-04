using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Helios_CloneOldDB.Models
{
    public class GetC2Context
    {
		public string ConnectionString { get; set; }

		public GetC2Context(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

    	public List<GetC2> GetListC2(String date)
        {
			List<GetC2> list = new List<GetC2>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select id" +
				                                    ",(SELECT url_landingpage from gd_campaign_landingpage where id = id_camp_landingpage) as tracking_link" +
				                                    ",datetime from gd_log where action = 'Visited' and  id_camp_landingpage != -100 limit 1;", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
						list.Add(new GetC2()
                        {
							id = reader["id"].ToString(),
							date_time = reader["datetime"].ToString(),
							TrackingLink = reader["tracking_link"].ToString()
                            //(long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds
                        });
                    }
                }
            }

            return list;
        }
    }
}
