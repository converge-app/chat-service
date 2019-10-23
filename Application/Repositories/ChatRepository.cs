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
        Task<IEnumerable<Message>> GetContactsForUserId(string userId);
        Task<List<Message>> GetAllContacts();

    }

    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Message> _chats;

        public ChatRepository(IDatabaseContext dbContext)
        {
            if (dbContext.IsConnectionOpen())
                _chats = dbContext.Chats;
        }

        public async Task<List<Message>> Get()
        {
            return await (await _chats.FindAsync(Chat => true)).ToListAsync();
        }

        public async Task<Message> GetById(string id)
        {
            return await (await _chats.FindAsync(Chat => Chat.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<Message> Create(Message @Message)
        {
            await _chats.InsertOneAsync(@Message);
            return @Message;
        }

        public async Task<IOrderedEnumerable<Message>> GetByContactId(string contactId)
        {
            var chats = await (await _chats.FindAsync(chat => chat.ContactId == contactId)).ToListAsync();
            return chats.OrderBy(e => e.Timestamp);
        }

        public async Task<IEnumerable<Message>> GetContactsForUserId(string userId)
        {
            var messages = await (await _chats.FindAsync(message => message.ReceiverId == userId || message.SenderId == userId)).ToListAsync();
            var contacts = messages.GroupBy(message => message.ContactId).Select(message => message.FirstOrDefault()).ToList();
            return contacts;
        }

        public async Task<List<Message>> GetAllContacts()
        {
            var messages = await (await _chats.FindAsync(message => true)).ToListAsync();
            var contacts = messages.GroupBy(message => message.ContactId).Select(message => message.FirstOrDefault()).ToList();
            return contacts;
        }

    }
}