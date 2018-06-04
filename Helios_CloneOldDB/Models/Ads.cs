using System;
using Helios_CloneOldDB.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Helios_CloneOldDB.Models
{
    public class Ads
    {
		[BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }
		[BsonElement("name")]
        public String name { get; set; }
		[BsonElement("medium")]
		public String medium { get; set; }
		[BsonElement("mol_tracking_link")]
		public String mol_tracking_link { get; set; }
		[BsonElement("source_id")]
		public String source_id { get; set; }
		[BsonElement("source_name")]
		public String source_name { get; set; }
		[BsonElement("team_id")]
		public String team_id { get; set; }
		[BsonElement("team_name")]
		public String team_name { get; set; }
		[BsonElement("campaign_name")]
		public String campaign_name { get; set; }
		[BsonElement("campaign_id")]
		public String campaign_id { get; set; }
        [BsonElement("subcampaign_id")]
		public String subcampaign_id { get; set; }
		[BsonElement("subcampaign_name")]
		public String subcampaign_name { get; set; }
		[BsonElement("landing_page_id")]
		public String landing_page_id { get; set; }
		[BsonElement("landing_page_name")]
		public String landing_page_name { get; set; }
		[BsonElement("creator_id")]
		public String creator_id { get; set; }
		[BsonElement("creator_name")]
		public String creator_name { get; set; }
		[BsonElement("tracking_link")]
		public String tracking_link { get; set; }
		[BsonElement("mol_link_tracking")]
		public String mol_link_tracking { get; set; }
		[BsonElement("uri_query")]
		public String uri_query { get; set; }

		//[BsonElement("shorten_url")]
		[BsonSerializer(typeof(TestingObjectTypeSerializer))]
		public String shorten_url { get; set; }

		//[BsonElement("is_active")]
		[BsonSerializer(typeof(TestingObjectTypeSerializer))]
		public String is_active { get; set; }
		//[BsonElement("updated_at")]
		//[BsonSerializer(typeof(TestingObjectTypeSerializer))]
		public DateTime updated_at { get; set; }
		//[BsonElement("created_at")]
		//[BsonSerializer(typeof(TestingObjectTypeSerializer))]
		public DateTime created_at { get; set; }

    }
}

public class TestingObjectTypeSerializer : IBsonSerializer
{
    public Type ValueType { get; } = typeof(string);

    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Int32) return GetNumberValue(context);

        return context.Reader.ReadString();
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        context.Writer.WriteString(value as string);
    }

    private static object GetNumberValue(BsonDeserializationContext context)
    {
        var value = context.Reader.ReadInt32();

        switch (value)
        {
            case 1:
                return "one";
            case 2:
                return "two";
            case 3:
                return "three";
            default:
                return "BadType";
        }
    }
}

