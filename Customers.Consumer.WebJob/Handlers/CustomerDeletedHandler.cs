using Customers.Consumer.WebJob.Models;
using MediatR;

namespace Customers.Consumer.WebJob.Handlers
{
    public sealed class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
    {
        private readonly ILogger<CustomerDeletedHandler> _logger;
        public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(CustomerDeleted request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.Id.ToString());
            return Task.CompletedTask;
        }
    }
}
