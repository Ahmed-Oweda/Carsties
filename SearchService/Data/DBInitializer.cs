using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;
using System.Text.Json;

namespace SearchService.Data
{
    public class DBInitializer
    {
        public static async Task InitilizeMongoDb(WebApplication app)
        {
            //inintilize mongo db and add index for searching
            await DB.InitAsync("SearchDB", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            //seed data if empty database
            var count = await DB.CountAsync<Item>();
            //if (count == 0)
            //{
            //    Console.WriteLine("No data - will attempt to do data seeding");
            //    var itemData = await File.ReadAllTextAsync("Data/auctions.json");

            //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            //    var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            //    await DB.SaveAsync(items);
            //}
            //else
            //{
            //    Console.WriteLine("data in mongo db exists .. no need for seeding");

            //}

            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();
            var items = await httpClient.GetitemsForSearchDb();
            Console.WriteLine(items.Count + " returned from the auction service");
            if(items.Count > 0)
                await DB.SaveAsync(items);
        }
    }
}
