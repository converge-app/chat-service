using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Models.Entities;
using Application.Repositories;
using Application.Utility.ClientLibrary;
using Application.Utility.ClientLibrary.Project;
using Application.Utility.Exception;
using Newtonsoft.Json;
/*sdjkfskjf */
namespace Application.Services
{
    public interface IChatService
    {
        Task<Message> AddContact(Message createMessage);
        Task<Message> PostMessage(Message sendMessage);
    }

    public class ChatService : IChatService
    {
        private readonly IChatRepository _ChatRepository;
        private readonly IClient _client;

        public ChatService(IChatRepository ChatRepository, IClient client)
        {
            _ChatRepository = ChatRepository;
            _client = client;
        }

        public async Task<Message> PostMessage(Message sendMessage)
        {

            Message message = new Message
            {
                SenderId = sendMessage.SenderId,
                Content = sendMessage.Content,
                ReceiverId = sendMessage.ReceiverId
            };
            message.ContactId = HashUsers(message.SenderId, message.ReceiverId);
            message.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return await _ChatRepository.Create(message);

        }

        public async Task<Message> AddContact(Message createMessage)
        {
            createMessage.ContactId = HashUsers(createMessage.SenderId, createMessage.ReceiverId);
            var contact = await _ChatRepository.GetByContactId(createMessage.ContactId);
            if (contact.Count() != 0)
            {
                throw new InvalidContact("Contact already added.");
            }

            createMessage.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            createMessage.Content = "Contact was added";

            return await _ChatRepository.Create(createMessage);
        }

        public async Task<List<Message>> GetChat(string senderId, string receiverId)
        {
            if (senderId == receiverId)
                throw new ArgumentException("Something went wrong");

            var hash = HashUsers(senderId, receiverId);

            var users = await _ChatRepository.GetByContactId(hash);
            return users.ToList();
        }

        private static string HashUsers(string senderId, string receiverId)
        {
            string hash;
            using (var md5 = MD5.Create())
            {
                var users = new List<string>();
                users.Add(senderId);
                users.Add(receiverId);
                users.Sort();
                var result = "";
                var resultSb = new StringBuilder();
                foreach (var user in users)
                {
                    resultSb.Append(user);

                }
                result = resultSb.ToString();
                hash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(result)));

            }

            return hash;
        }
    }
}