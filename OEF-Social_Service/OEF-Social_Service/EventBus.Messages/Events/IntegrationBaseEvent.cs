namespace EventBus.Messages.Events
{
    public abstract class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            EventId = string.Empty;
            EventCreationDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvent(string id, DateTime createDate)
        {
            EventId = id;
            EventCreationDate = createDate;
        }

        public string EventId { get; private set; }
        public DateTime EventCreationDate { get; private set; }
    }
}