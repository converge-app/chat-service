using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Database;
using Application.Models.Entities;
using MongoDB.Driver;

namespace Application.Repositories
{
    public interface IChatRepository
    {
        Task<List<Event>> Get();
        Task<Event> GetById(string id);
        Task<Event> Create(Event @event);
        Task<IOrderedEnumerable<Event>> GetByProjectId(string projectId);
    }

    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Event> _Chats;

        public ChatRepository(IDatabaseContext dbContext)
        {
            if (dbContext.IsConnectionOpen())
                _Chats = dbContext.Chats;
        }

        public async Task<List<Event>> Get()
        {
            return await (await _Chats.FindAsync(Chat => true)).ToListAsync();
        }

        public async Task<Event> GetById(string id)
        {
            return await (await _Chats.FindAsync(Chat => Chat.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<Event> Create(Event @event)
        {
            await _Chats.InsertOneAsync(@event);
            return @event;
        }

        public async Task<IOrderedEnumerable<Event>> GetByProjectId(string projectId)
        {
            var events = await (await _Chats.FindAsync(@event => @event.ProjectId == projectId)).ToListAsync();
            return events.OrderByDescending(e => e.Timestamp);
        }
    }
}