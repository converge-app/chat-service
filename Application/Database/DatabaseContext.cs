using Application.Models;
using Application.Models.Entities;
using Application.Utility.Database;
using MongoDB.Driver;

namespace Application.Database
{
    public interface IDatabaseContext
    {
        IMongoCollection<Message> Chats { get; }

        bool IsConnectionOpen();
    }

    public class DatabaseContext : IDatabaseContext
    {
        private readonly IMongoDatabase _database;

        public DatabaseContext(IDatabaseSettings settings)
        {
            var mongoSettings = settings.GetSettings();
            var client = new MongoClient(mongoSettings);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Message> Chats => _database.GetCollection<Message>("Chats");

        public bool IsConnectionOpen()
        {
            return _database != null;
        }
    }
}