using Customers.Consumer.WebJob.Abstractions;

namespace Customers.Consumer.WebJob.Models
{
    public sealed class CustomerCreatedMessage : IMessageSqs
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string GitHubUserName { get; init; }
    }
    public sealed class CustomerUpdatedMessage : IMessageSqs
    {
        public string? Name { get; init; }
        public string? Email { get; init; }
        public string? GitHubUserName { get; init; }
    }
    public sealed class CustomerDeletedMessage : IMessageSqs
    {
        public Guid Id { get; init; }
    }
    // UPDATE, DELETE 
}
