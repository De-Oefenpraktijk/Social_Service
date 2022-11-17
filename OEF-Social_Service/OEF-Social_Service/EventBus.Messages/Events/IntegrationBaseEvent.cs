namespace EventBus.Messages.Events
{
    public abstract class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            EventId = Guid.NewGuid();
            EventCreationDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvent(Guid id, DateTime createDate)
        {
            EventId = id;
            EventCreationDate = createDate;
        }

        public Guid EventId { get; private set; }
        public DateTime EventCreationDate { get; private set; }
    }
}