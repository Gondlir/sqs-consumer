using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.WebJob.Abstractions;
using Customers.Consumer.WebJob.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading;

namespace Customers.Consumer.WebJob
{
    public sealed class QueueConsumerService : BackgroundService
    {
        private const string _url = "customers";       
        private readonly IMediator _mediator;
        private static AmazonSQSClient _awsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
        private readonly IOptions<QueueSettings> _queuSettings;
        private readonly ILogger<QueueConsumerService> _logger;
        public QueueConsumerService(IAmazonSQS amazonSQS, IOptions<QueueSettings> queuSettings, 
            IMediator mediator, ILogger<QueueConsumerService> logger)
        {
            _queuSettings = queuSettings;
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueUrlReponse = await _awsClient.GetQueueUrlAsync(_queuSettings.Value.Name);
            await ProcessQueueCustomerMessageAsync(queueUrlReponse, stoppingToken);
        }
        #region Private Methods
        private async Task ProcessQueueCustomerMessageAsync(GetQueueUrlResponse queueUrlReponse, CancellationToken cancellationToken)
        {
            var receivedMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrlReponse.QueueUrl,
                AttributeNames = new List<string> { "All" },
                MessageAttributeNames = new List<string> { "All" },
                MaxNumberOfMessages = 1
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _awsClient.ReceiveMessageAsync(receivedMessageRequest, cancellationToken);
                   
                    foreach (var message in response.Messages)
                    {
                        var messageType = message.MessageAttributes["MessageType"].StringValue;
                        var type = Type.GetType($"Customers.Consumer.WebJob.Models.{messageType}");
                        if(type is null) 
                        {
                            _logger.LogWarning("Tipo de mensagem não encontrado: {MessageType}", messageType);
                            continue;
                        }
                        var typedMessage = (IMessageSqs)JsonSerializer.Deserialize(message.Body, type)!;

                        try
                        {
                            await _mediator.Send(typedMessage, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Houve uma falha no processamento da mensagem");
                            continue;
                        }
                      
                        await _awsClient.DeleteMessageAsync(queueUrlReponse.QueueUrl, message.ReceiptHandle);
                    }
                    await Task.Delay(3000, cancellationToken);
                   
                }
                catch (Exception ex)
                {
                    // Log errors
                    _logger.LogError(ex, $"Houve uma falha no processamento da mensagem... " +
                        $"InnerException: { ex.InnerException }, StackTrace: { ex.StackTrace }");
                    throw;
                }

            }
        }
        #endregion
    }
}
