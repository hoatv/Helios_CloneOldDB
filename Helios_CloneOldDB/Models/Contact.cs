using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helios_CloneOldDB.Models
{
    public class Contact
    {
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId _id { get; set; }
		[BsonElement("domain")]
		public String Domain { get; set; }
		[BsonElement("msg_type")]
		public String MsgType { get; set; }
		[BsonElement("contact_id")]
		public String ContactID { get; set; }
		//public String contact_id { get; set;}
		[BsonElement("email")]
		public String Email { get; set; }
		[BsonElement("session_code")]
		public String SessionCode { get; set; }
		[BsonElement("phone")]
		public String Phone { get; set; }
		[BsonElement("clevel")]
		public String Clevel { get; set; }
		[BsonElement("name")]
		public String Name { get; set; }
    	[BsonElement("code_chanel")]
		public String CodeChanel { get; set; }
		[BsonElement("age")]
		public String Age { get; set; }
		[BsonElement("ad_id")]
		public String AdID { get; set; }
		[BsonElement("submit_time")]
		public String SubmitTime { get; set; }
		[BsonElement("current_level")]
		public String CurrentLevel { get; set; }
		[BsonElement("is_old_data")]
		public Boolean IsOldData { get; set; }
		//[BsonElement("tracking")]
        public String TrackingLink { get; set; }
		//public String AdID { get; set; }
		public String old_tracking_link { get; set; }
        
      }
}
