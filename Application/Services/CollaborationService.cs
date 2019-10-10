using System;
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
        Task<Event> Create(Event createEvent);
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

        public async Task<Event> Create(Event createEvent)
        {
            if (JsonConvert.DeserializeObject(createEvent.Content) == null)
                throw new InvalidEvent("Content was not parseable");

            if (await _client.GetProjectAsync(createEvent.ProjectId) == null)
                throw new ProjectNotFound();

            createEvent.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return await _ChatRepository.Create(createEvent);
        }
    }
}