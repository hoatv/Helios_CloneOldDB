using System;
using MongoDB.Bson.IO;
namespace Helios_CloneOldDB.Models
{
	public class BsonStringSerializer : BsonBaseSerializer
    {
		private static BsonStringSerializer __instance = new BsonStringSerializer();

        // constructors
        /// <summary>
        /// Initializes a new instance of the BsonStringSerializer class.
        /// </summary>
        public BsonStringSerializer()
        {
        }

        // public static properties
        /// <summary>
        /// Gets an instance of the BsonStringSerializer class.
        /// </summary>
        public static BsonStringSerializer Instance
        {
            get { return __instance; }
        }

        // public methods
        /// <summary>
        /// Deserializes an object from a BsonReader.
        /// </summary>
        /// <param name="bsonReader">The BsonReader.</param>
        /// <param name="nominalType">The nominal type of the object.</param>
        /// <param name="actualType">The actual type of the object.</param>
        /// <param name="options">The serialization options.</param>
        /// <returns>An object.</returns>
        public override object Deserialize(
            BsonReader bsonReader,
            Type nominalType,
            Type actualType,
            IBsonSerializationOptions options)
        {
            VerifyTypes(nominalType, actualType, typeof(BsonString));

            var bsonType = bsonReader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.String:
                    return new BsonString(bsonReader.ReadString());
                default:
                    var message = string.Format("Cannot deserialize BsonString from BsonType {0}.", bsonType);
                    throw new FileFormatException(message);
            }
        }

        /// <summary>
        /// Serializes an object to a BsonWriter.
        /// </summary>
        /// <param name="bsonWriter">The BsonWriter.</param>
        /// <param name="nominalType">The nominal type.</param>
        /// <param name="value">The object.</param>
        /// <param name="options">The serialization options.</param>
        public override void Serialize(
            BsonWriter bsonWriter,
            Type nominalType,
            object value,
            IBsonSerializationOptions options)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var bsonString = (BsonString)value;
            bsonWriter.WriteString(bsonString.Value);
        }
    }
}
