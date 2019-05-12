using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Entities.Migrations
{
	public static class AppMigration
	{

		public static async Task Run(IMongoClient mongoDbClient)
		{
			await AppMigration.MakeSureDatabaseCreated(mongoDbClient);
			await AppMigration.TestRunScript(mongoDbClient);
		}

		private static async Task MakeSureDatabaseCreated(IMongoClient mongoDbClient)
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

					//var bookCollection = database.GetCollection<Book>("Book");

					//bookCollection.InsertOne(new Book
					//{
					//	Name = "First book",
					//	Author = "Tan Nguyen",
					//	Category = "No name",
					//	CreatedAt = DateTime.Now,
					//	CreatedBy = "Tan"
					//});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				//
				// TODO: Handle can't make sure database created
				throw;
			}
		}

		/// <summary>
		/// Execute mongodb script with dotnet core
		/// </summary>
		/// <param name="mongoDbClient"></param>
		/// <returns></returns>
		private static async Task TestRunScript(IMongoClient mongoDbClient)
		{
			try
			{
				var database = mongoDbClient.GetDatabase("BookStoreDB");

				const string script = @"db.Book.update({}, {$rename:{'Name':'BookName'}}, false, true)";

				// db.RunCommand takes JSON as a parameter (docs) while you're trying to pass your script directly.
				// You can pass your script as a value where key is set to eval.
				// https://docs.mongodb.com/manual/reference/command/eval/#example
				// Why we need eval: https://jira.mongodb.org/browse/SERVER-17453?focusedCommentId=843013&page=com.atlassian.jira.plugin.system.issuetabpanels%3Acomment-tabpanel#comment-843013
				var doc = new BsonDocument()
				{
					{"eval", script}
				};
				var command = new BsonDocumentCommand<BsonDocument>(doc);

				await database.RunCommandAsync(command);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				// TODO: Handle execute script error
				throw;
			}
		}


		/// <summary>
		/// Check is database has exists collection
		/// </summary>
		/// <param name="database"></param>
		/// <param name="collectionName"></param>
		/// <returns></returns>
		private static async Task<bool> ExistsCollection(this IMongoDatabase database, string collectionName)
		{
			var filter = new BsonDocument("name", collectionName);
			var collectionCursor = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
			return await collectionCursor.AnyAsync();
		}
	}
}
