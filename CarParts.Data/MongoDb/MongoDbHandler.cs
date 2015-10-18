namespace CarParts.Data.MongoDb
{
    using System.Collections.Generic;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoDbHandler
    {
        private const string DatabaseName = "carparts";

        private const string ConnectionString = @"mongodb://TeamXenon:xenon@ds041144.mongolab.com:41144/carparts";

        private readonly string databaseName;
        private readonly string connectionString;

        public MongoDbHandler()
            : this(ConnectionString)
        {
        }

        public MongoDbHandler(string connectionString)
        {
            this.connectionString = connectionString;
            this.databaseName = DatabaseName;
        }

        public void WriteCollection<T>(string collectionName, IEnumerable<T> collectionItems)
        {
            var database = this.GetDatabase(this.databaseName);
            MongoCollection<T> collection = database.GetCollection<T>(collectionName);

            foreach (var item in collectionItems)
            {
                collection.Insert(item);
            }
        }

        public IEnumerable<BsonDocument> ReadCollection(string collectionName)
        {
            var database = this.GetDatabase(this.databaseName);
            var collection = database.GetCollection(collectionName);

            return collection.FindAll();
        }

        private MongoDatabase GetDatabase(string databaseName)
        {
            var mongoClient = new MongoClient(this.connectionString);
            var mongoServer = mongoClient.GetServer();

            var database = mongoServer.GetDatabase(databaseName);
            return database;
        }
    }
}
