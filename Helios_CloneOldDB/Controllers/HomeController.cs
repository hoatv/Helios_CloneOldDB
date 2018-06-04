using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Helios_CloneOldDB.Models;
using MySql.Data.MySqlClient;
using MongoDB.Driver;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MongoDB.Bson;
using System.Reflection.Metadata;

namespace Helios_CloneOldDB.Controllers
{
	public class HomeController : Controller
	{
		private IMongoDatabase mongoDatabase;
		public IMongoDatabase GetMongoDatabase()
		{
			//var mongoClient = new MongoClient("mongodb://localhost:27017");
			var mongoClient = new MongoClient("mongodb://42.115.221.30:27017");
			return mongoClient.GetDatabase("helios");
		}

		public IActionResult Index()
		{
			ContactContext context = HttpContext.RequestServices.GetService(typeof(ContactContext)) as ContactContext;

			List<Contact> list = new List<Contact>();
			List<Contact> listInsert = new List<Contact>();
			list = context.GetListOldContactByDate("");

			System.Console.WriteLine("------------C3->START--------------");
            
			mongoDatabase = GetMongoDatabase();
			foreach (var item in list)
			{
				// remove first ZERO of old MOL
				if (!string.IsNullOrEmpty(item.Phone))
				{
					if (item.Phone.Length >= 10)
					{
						item.Phone = item.Phone.Substring(1);
					}
				}

				string linkTrackingLinkToFind = string.Empty;
				if (!string.IsNullOrEmpty(item.TrackingLink))
				{
					int i = item.TrackingLink.IndexOf('?');
					linkTrackingLinkToFind = item.TrackingLink.Substring(i + 1);
				}

				var filter = new BsonDocument { { "tracking_link", new BsonDocument { { "$regex", linkTrackingLinkToFind }, { "$options", "i" } } } };

				Ads ads = mongoDatabase.GetCollection<Ads>("ads").Find(filter).FirstOrDefault();
				item.old_tracking_link = item.TrackingLink;
				if(ads != null){
					item.AdID = ads._id;
				}else{
					item.AdID = "unknown";
				}


				//Instert Contacts to mongodb
				mongoDatabase.GetCollection<Contact>("contacts").InsertOne(item);

				// Upsert Ad_results
				// Handle date time
				double ticks = double.Parse(item.SubmitTime);
				TimeSpan time = TimeSpan.FromMilliseconds(ticks);
				DateTime startdate = new DateTime(1970, 1, 1) + time;
				//if (!string.IsNullOrEmpty(item.AdID))
				//{
				//	item.AdID = ads._id;
				//}else {
				//	item.AdID = "unknown";
				//}
                
				System.Console.WriteLine("ad_id:"+ item.AdID);
				System.Console.WriteLine("date:"+ startdate.ToString("yyyy-MM-dd"));
				FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
				FilterDefinition<BsonDocument> filters = builder.Eq("ad_id",item.AdID) & builder.Eq("date", startdate.ToString("yyyy-MM-dd"));
				var update = Builders<BsonDocument>.Update.Inc("c3", 1).Inc("c3b",1).Push("c3_old",item.Phone);
				var options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>()
				{IsUpsert = true };

				var checkExists = mongoDatabase.GetCollection<BsonDocument>("ad_results").
				                               FindOneAndUpdate(filters,update, options);

			}
			System.Console.WriteLine("------------C3->END--------------");
			System.Console.WriteLine("------------C2->START--------------");
            
			/// Handle C2 => ++ 1 in ad_results
            GetC2Context contextC2 = HttpContext.RequestServices.GetService(typeof(GetC2Context)) as GetC2Context;

            List<GetC2> listC2 = new List<GetC2>();
            listC2 = contextC2.GetListC2("");

            foreach (var item in listC2)
            {

				string linkTrackingLinkToFind = string.Empty;
                if (!string.IsNullOrEmpty(item.TrackingLink))
                {
                    int i = item.TrackingLink.IndexOf('?');
                    linkTrackingLinkToFind = item.TrackingLink.Substring(i + 1);
                }

                var filter = new BsonDocument { { "tracking_link", new BsonDocument { { "$regex", linkTrackingLinkToFind }, { "$options", "i" } } } };

                Ads ads = mongoDatabase.GetCollection<Ads>("ads").Find(filter).FirstOrDefault();
                //item.old_tracking_link = item.TrackingLink;
				if (ads != null)
                {
					item.ad_id = ads._id;
				}else{
					item.ad_id = "unknown";
				}


				FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
				FilterDefinition<BsonDocument> filters = builder.Eq("ad_id", item.ad_id) & builder.Eq("date", DateTime.Parse(item.date_time).ToString("yyyy-MM-dd"));
				var update = Builders<BsonDocument>.Update.Inc("c2", 1).Push("c2_old_id",item.id);
                var options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>()
                { IsUpsert = true };

                var checkExists = mongoDatabase.GetCollection<BsonDocument>("ad_results").
                                               FindOneAndUpdate(filters, update, options);

            }
			System.Console.WriteLine("------------C2->END--------------");
            
			return View();
		}




	}
}
