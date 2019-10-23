using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Helpers;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Repositories;
using Application.Services;
using Application.Utility;
using Application.Utility.Exception;
using Application.Utility.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace Application.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _ChatRepository;
        private readonly IChatservice _chatService;

        public ChatsController(IChatservice Chatservice, IChatRepository ChatRepository, IMapper mapper)
        {
            _chatService = Chatservice;
            _ChatRepository = ChatRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] MessageCreationDto MessageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var createMessage = _mapper.Map<Message>(MessageDto);
            try
            {
                var createdMessage = await _chatService.PostMessage(createMessage);
                return Ok(createdMessage);
            }
            catch (UserNotFound)
            {
                return NotFound(new MessageObj("User not found"));
            }
            catch (EnvironmentNotSet)
            {
                throw;
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }
        }


        [HttpPost("contacts")]
        public async Task<IActionResult> AddContact([FromBody] AddContactDTO AddContactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var createMessage = _mapper.Map<Message>(AddContactDto);
            try
            {
                var createdMessage = await _chatService.AddContact(createMessage);
                return Ok(createdMessage);
            }
            catch (ProjectNotFound)
            {
                return NotFound(new MessageObj("Project not found"));
            }
            catch (EnvironmentNotSet)
            {
                throw;
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Chats = await _ChatRepository.Get();
            var ChatDtos = _mapper.Map<IList<MessageDto>>(Chats);
            return Ok(ChatDtos);
        }

        [HttpGet("contacts/{contactId}")]
        public async Task<IActionResult> GetByContactId([FromRoute] string contactId)
        {
            var messages = (await _ChatRepository.GetByContactId(contactId)).ToList();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var Chat = await _ChatRepository.GetById(id);
            var ChatDto = _mapper.Map<MessageDto>(Chat);
            return Ok(ChatDto);
        }

        [HttpGet("contacts/user/{userId}")]
        public async Task<IActionResult> GetContactsForUserId([FromRoute] string userId)
        {
            var messages = await _ChatRepository.GetContactsForUserId(userId);
            return Ok(messages);
        }
    }
}