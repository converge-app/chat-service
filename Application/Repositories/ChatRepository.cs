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
        Task<List<Message>> Get();
        Task<Message> GetById(string id);
        Task<Message> Create(Message @Message);
          Task<IOrderedEnumerable<Message>> GetByContactId(string contactId);
    }

    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Message> _Chats;

        public ChatRepository(IDatabaseContext dbContext)
        {
            if (dbContext.IsConnectionOpen())
                _Chats = dbContext.Chats;
        }

        public async Task<List<Message>> Get()
        {
            return await (await _Chats.FindAsync(Chat => true)).ToListAsync();
        }

        public async Task<Message> GetById(string id)
        {
            return await (await _Chats.FindAsync(Chat => Chat.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<Message> Create(Message @Message)
        {
            await _Chats.InsertOneAsync(@Message);
            return @Message;
        }

             public async Task<IOrderedEnumerable<Message>> GetByContactId(string contactId)
        {
            var chats = await (await _Chats.FindAsync(chat => chat.ContactId == contactId)).ToListAsync();
            return chats.OrderByDescending(e => e.Timestamp);
        }
    }
}