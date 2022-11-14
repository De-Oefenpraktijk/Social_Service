using AutoMapper;
using MassTransit.Mediator;
using MassTransit;
using EventBus.Messages.Events;
using OEF_Social_Service.Models;
using OEF_Social_Service.DataAccess.Data.Services;

namespace OEF_Social_Service.EventBus.Consumer
{
    public class ProfileUpdatedConsumer : IConsumer<ProfileUpdatedEvent>
    {
        private readonly ILogger<ProfileUpdatedConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly DataAccess.Data.Services.Interfaces.IFollowService _followService;

        public ProfileUpdatedConsumer(ILogger<ProfileUpdatedConsumer> logger, IMapper mapper, DataAccess.Data.Services.Interfaces.IFollowService followService)
        {
            _logger = logger;
            _mapper = mapper;
            _followService = followService;
        }

        public async Task Consume(ConsumeContext<ProfileUpdatedEvent> context)
        {
            var entity = _mapper.Map<Person>(context.Message);

            _logger.Log(LogLevel.Information, "", entity.FirstName);

            var user = _followService.GetUser(entity.Username).Result;
            if (user.Length == 2)
            {
                await _followService.CreateUser(entity);
                return;
            }
            await _followService.UpdateUser(entity);
        }
    }
}
