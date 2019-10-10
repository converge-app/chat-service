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

namespace Application.Services
{
    public interface IChatservice
    {
        Task<Message> AddContact(Message createMessage);
    }

    public class Chatservice : IChatservice
    {
        private readonly IChatRepository _ChatRepository;
        private readonly IClient _client;

        public Chatservice(IChatRepository ChatRepository, IClient client)
        {
            _ChatRepository = ChatRepository;
            _client = client;
        }

        public async Task<Message> AddContact(Message createMessage)
        {


            var hash = "";
            hash = HashUsers(createMessage.SenderId, createMessage.RecieverId);

            createMessage.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

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