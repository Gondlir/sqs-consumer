using Customers.Consumer.WebJob.Models;
using MediatR;

namespace Customers.Consumer.WebJob.Handlers
{
    public sealed class CustomerCreatedHandler : IRequestHandler<CustomerCreatedMessage>
    {
        private readonly ILogger<CustomerCreatedHandler> _logger;

        public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CustomerCreatedMessage request, CancellationToken cancellationToken)
        {
            // do stuffs 
            _logger.LogInformation(request.Name);
            return Task.CompletedTask;
        }
    }
}
