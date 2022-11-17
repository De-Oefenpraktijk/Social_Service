using AutoMapper;
using EventBus.Messages.Events;
using OEF_Social_Service.Models;

namespace OEF_Social_Service.Mapper
{
    public class PersonMapper : Profile
    {
        public PersonMapper()
        {
            CreateMap<Person, ProfileUpdatedEvent>().ReverseMap();
        }
    }
}
