using Customers.Consumer.WebJob.Models;
using MediatR;

namespace Customers.Consumer.WebJob.Handlers
{
    public sealed class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
    {
        private readonly ILogger<CustomerUpdatedHandler> _logger;

        public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.Email);
            return Task.CompletedTask;
        }
    }
}
