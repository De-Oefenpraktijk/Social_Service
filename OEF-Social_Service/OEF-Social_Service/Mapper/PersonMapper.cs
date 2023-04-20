using AutoMapper;
using EventBus.Messages.Events;
using OEF_Social_Service.Models;

namespace OEF_Social_Service.Mapper
{
    public class NotFoundException : Profile
    {
        public NotFoundException()
        {
            CreateMap<Person, ProfileUpdatedEvent>().ReverseMap();
        }
    }
}
