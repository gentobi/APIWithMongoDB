using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Entities.Migrations
{
	public static class AppMigration
	{
		public static async Task MakeSureDatabaseCreated(IMongoClient mongoDbClient)
		{
			try
			{
				var database = mongoDbClient.GetDatabase("BookStoreDB");

				using (var session = await mongoDbClient.StartSessionAsync())
				{
					var collections = DbCollection.All();

					foreach (var collectionName in collections)
					{
						if (await database.ExistsCollection(collectionName))
							continue;

						await database.CreateCollectionAsync(session, collectionName);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private static async Task<bool> ExistsCollection(this IMongoDatabase database, string collectionName)
		{
			var filter = new BsonDocument("name", collectionName);
			var collectionCursor = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
			return await collectionCursor.AnyAsync();
		}
	}
}
