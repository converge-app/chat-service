using Application.Models.DataTransferObjects;
using Application.Models.Entities;

namespace Application.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>();
            CreateMap<MessageCreationDto, Message>();
            CreateMap<AddContactDTO, Message>();
            CreateMap<Message, AddContactDTO>();
        }
    }
}