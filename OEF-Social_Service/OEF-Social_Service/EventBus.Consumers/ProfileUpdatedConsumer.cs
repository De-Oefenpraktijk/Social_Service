using AutoMapper;
using MassTransit.Mediator;
using MassTransit;
using EventBus.Messages.Events;

namespace OEF_Social_Service.EventBus.Consumer
{
    public class ProfileUpdatedConsumer : IConsumer<ProfileUpdatedEvent>
    {
        private readonly ILogger<ProfileUpdatedConsumer> _logger;

        public ProfileUpdatedConsumer(ILogger<ProfileUpdatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProfileUpdatedEvent> context)
        {
            //var entity = _mapper.Map<KweetEntity>(context.Message);
            // TODO: implement code to consume
            // context.message

        }
    }
}
