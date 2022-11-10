namespace EventBus.Messages.Events
{
    public class ProfileUpdatedEvent: IntegrationBaseEvent
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // used as unique identifier
        public string Username { get; set; } = null!;

        public string? EmailAddress { get; set; }

        public string? Password { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public string? Role { get; set; }

        public string? Institutions { get; set; }

        public string? Themes { get; set; }

        public string? ResidencePlace { get; set; }
    }
}
