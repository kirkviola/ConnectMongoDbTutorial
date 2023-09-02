using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Runtime.CompilerServices;

var connectionString = "mongodb://localhost:27017";
var client = new MongoClient(connectionString);

IMongoDatabase db = client.GetDatabase("Experimental");

var collection = db.GetCollection<Chat>("Chats");

var docs = await collection.EstimatedDocumentCountAsync();

Console.WriteLine($"This collection has {docs} records.");

var query = from chat in collection.AsQueryable()
            where chat.Name == "Matt"
            select chat;

foreach (var result in query)
{
    Console.WriteLine($"Id: {result.Id}");
    Console.WriteLine("{0}: {1}", result.Name, result.Message);
}

var newChat = new Chat()
{
    Name = "John",
    Message = "Hello? Is this thing working?"
};

await collection.InsertOneAsync(newChat);

var newEntry = await collection.AsQueryable().FirstOrDefaultAsync(x => x.Name == "John");

if (newEntry != null)
    Console.WriteLine($"{newEntry.Name} says {newEntry.Message}");
public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("message")]
    public string Message { get; set; } = string.Empty;

    // There is a BsonIgnoreIfNull attribute as well

    public Chat() { }
}

// Commenting here to have a record of which packages to install:
// MongoDB.Bson, MongoDB.Driver, MongoDB.Driver.Core
