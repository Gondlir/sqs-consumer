using Customers.Consumer.WebJob.Abstractions;

namespace Customers.Consumer.WebJob.Models
{
    public sealed class CustomerCreated : IMessageSqs
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string GitHubUsername { get; init; }
    }
    public sealed class CustomerUpdated : IMessageSqs
    {
        public string? Name { get; init; }
        public string? Email { get; init; }
        public string? GitHubUsername { get; init; }
    }
    public sealed class CustomerDeleted : IMessageSqs
    {
        public Guid Id { get; init; }
    }
}
