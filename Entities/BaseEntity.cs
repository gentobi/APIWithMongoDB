using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Entities
{
	public class BaseEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("CreatedAt")]
		public DateTime CreatedAt { get; set; }
		[BsonElement("CreatedBy")]
		public string CreatedBy { get; set; }

		[BsonElement("UpdatedAt")]
		public DateTime? UpdatedAt { get; set; }
		[BsonElement("UpdatedBy")]
		public string UpdatedBy { get; set; }

		[BsonElement("IsDeleted")]
		public bool IsDeleted { get; set; }
	}
}
