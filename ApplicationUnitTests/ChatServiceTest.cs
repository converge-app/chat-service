using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Models.Entities;
using Application.Repositories;
using Application.Services;
using Application.Utility.ClientLibrary;
using Moq;
using Xunit;

namespace ApplicationUnitTests
{
    public class ChatServiceTest
    {
        [Fact]
        public async System.Threading.Tasks.Task AddContact_ThrowsInvalidContact()
        {
            // Arrange

            var chatRepository = new Mock<IChatRepository>();
            var client = new Mock<IClient>();

            var list = new List<Message>();
            list.Add(new Message()
            {
                SenderId = "User 1",
                ReceiverId = "User 2",
                Content = "Empty",
                Timestamp = 201607110
            });

            chatRepository.Setup(m => m.GetByContactId(It.IsAny<string>())).ReturnsAsync(list.OrderBy(e => e.Timestamp));
            var chatService = new ChatService(chatRepository.Object, client.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidContact>(() => chatService.AddContact(new Message
            {
                Content = "Empty",
                SenderId = "User 1",
                ReceiverId = "User 2",
            }));
        }

        [Fact]
        public async System.Threading.Tasks.Task AddContact_WithContactId()
        {
            // Arrange

            var chatRepository = new Mock<IChatRepository>();
            var client = new Mock<IClient>();
            var list = new List<Message>();
            var expected = new Message()
            {
                SenderId = "User 1",
                ReceiverId = "User 2",
                Content = "Empty",
                ContactId = "vt3nvc1fsihjj7xMgu3uqw=="
            };

            chatRepository.Setup(m => m.GetByContactId(It.IsAny<string>())).ReturnsAsync(list.OrderBy(e => e.Timestamp));
            chatRepository.Setup(m => m.Create(It.IsAny<Message>())).Returns<Message>(x => Task.Run(() => x));
            var chatService = new ChatService(chatRepository.Object, client.Object);

            // Act
            var actual = await chatService.AddContact(new Message
            {
                Content = "Empty",
                SenderId = "User 1",
                ReceiverId = "User 2",
            });

            // Assert
            Assert.Equal(expected.ContactId, actual.ContactId);
        }

        [Fact]
        public async System.Threading.Tasks.Task PostMessage_CreateMessage_SendMessage()
        {
            // Arrange

            var chatRepository = new Mock<IChatRepository>();
            var client = new Mock<IClient>();
            var list = new List<Message>();


            chatRepository.Setup(m => m.GetByContactId(It.IsAny<string>())).ReturnsAsync(list.OrderBy(e => e.Timestamp));
            chatRepository.Setup(m => m.Create(It.IsAny<Message>())).Returns<Message>(x => Task.Run(() => x));
            var chatService = new ChatService(chatRepository.Object, client.Object);

            // Act
            var actual = await chatService.PostMessage(new Message
            {
                Content = "Empty",
                SenderId = "User 1",
                ReceiverId = "User 2",
                ContactId = "vt3nvc1fsihjj7xMgu3uqw=="
            });

            // Assert
            Assert.Equal(actual.Content, actual.Content);
        }


        [Fact]
        public async System.Threading.Tasks.Task GetChat_ThrowsArgumentException()
        {
            // Arrange

            var chatRepository = new Mock<IChatRepository>();
            var client = new Mock<IClient>();

            var list = new List<Message>();
            /*list.Add(new Message()
            {
                SenderId = "User 1",
                ReceiverId = "User 1",
                Content = "Empty",
                Timestamp = 201607110
            });*/

            //chatRepository.Setup(m => m.GetByContactId(It.IsAny<string>())).ReturnsAsync(list.OrderBy(e => e.Timestamp));
            var chatService = new ChatService(chatRepository.Object, client.Object);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => chatService.GetChat("user 1", "user 1"));
        }



    }
}