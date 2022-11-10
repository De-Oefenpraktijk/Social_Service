using AutoMapper;
using MassTransit.Mediator;
using MassTransit;
using EventBus.Messages.Events;

namespace OEF_Social_Service.EventBus.Consumer
{
    public class ProfileUpdatedConsumer : IConsumer<ProfileUpdatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<ProfileUpdatedConsumer> _logger;

        public ProfileUpdatedConsumer(IMapper mapper, IMediator mediator, ILogger<ProfileUpdatedConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProfileUpdatedEvent> context)
        {
            //var entity = _mapper.Map<KweetEntity>(context.Message);

        }
    }
}
