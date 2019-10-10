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
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _ChatRepository;
        private readonly IChatservice _Chatservice;

        public ChatsController(IChatservice Chatservice, IChatRepository ChatRepository, IMapper mapper)
        {
            _Chatservice = Chatservice;
            _ChatRepository = ChatRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] EventCreationDto eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var createEvent = _mapper.Map<Event>(eventDto);
            try
            {
                var createdEvent = await _Chatservice.Create(createEvent);
                return Ok(createdEvent);
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var Chats = await _ChatRepository.Get();
            var ChatDtos = _mapper.Map<IList<EventDto>>(Chats);
            return Ok(ChatDtos);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId([FromRoute] string projectId)
        {
            var events = (await _ChatRepository.GetByProjectId(projectId)).ToList();
            return Ok(events);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var Chat = await _ChatRepository.GetById(id);
            var ChatDto = _mapper.Map<EventDto>(Chat);
            return Ok(ChatDto);
        }
    }
}