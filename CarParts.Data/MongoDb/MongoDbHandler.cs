namespace CarParts.Data.MongoDb
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CarParts.Models.MongoDbModels;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    public class MongoDbHandler
    {
        private const string DatabaseName = "carparts";

        private const string ConnectionString = @"mongodb://TeamXenon:xenon@ds041144.mongolab.com:41144/carparts";

        private readonly string databaseName;
        private readonly string connectionString;
        private IMongoDatabase database;

        public MongoDbHandler()
            : this(ConnectionString)
        {
        }

        public MongoDbHandler(string connectionString)
        {           
            this.connectionString = connectionString;
            this.databaseName = DatabaseName;
            this.database = this.GetDatabase(this.databaseName);
        }

        public async void WriteCollection(string collectionName, IEnumerable<BsonDocument> collectionItems)
        {
            var collection = this.database.GetCollection<BsonDocument>(collectionName);

            await collection.InsertManyAsync(collectionItems);
        }

        public async Task<List<BsonDocument>> ReadCollection(string collectionName)
        {
            var collection = this.database.GetCollection<BsonDocument>(collectionName);

            List<BsonDocument> list = await collection.Find(_ => true).ToListAsync();

            return list;
        }

        private IMongoDatabase GetDatabase(string databaseName)
        {
            BsonClassMap.RegisterClassMap<PartCategory>();

            var mongoClient = new MongoClient(this.connectionString);

            var database = mongoClient.GetDatabase(databaseName);
            return database;
        }
    }
}