using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Helios_CloneOldDB.Models
{
    public class ContactContext
    {
		public string ConnectionString { get; set; }

		public ContactContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

		public List<Contact> GetListOldContactByDate(String date)
        {
            List<Contact> list = new List<Contact>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
				MySqlCommand cmd = new MySqlCommand("select contact_id ,(SELECT url_landingpage from gd_campaign_landingpage where id = id_camp_landingpage) as tracking_link"
				                                    +", email, session, phone , name, age, datetime_submitted from dm_contact where id_camp_landingpage != -100 limit 1;", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Contact()
                        {
                            ContactID = reader["contact_id"].ToString(),
							TrackingLink = reader["tracking_link"].ToString(),
                            Email = reader["email"].ToString(),
							SessionCode = reader["session"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Name = reader["name"].ToString(),
							Age = reader["age"].ToString(),
							SubmitTime = (DateTime.Parse(reader["datetime_submitted"].ToString()).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString(),
							Clevel = "c3b",
							IsOldData = true
								//(long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds
                        });
                    }
                }
            }
   
            return list;
        }


    }
}
