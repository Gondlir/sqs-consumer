using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.SQS;
using Amazon.SQS.Model;
using Sqs.Consumer.WebJob.Models;
using System.Runtime.InteropServices;

namespace Sqs.Consumer.WebJob
{
    public sealed class EventAWSConsumer
    {
        private static string _url = "customers";
        private static AmazonSQSClient _awsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
        private static CustomerCreatedMessage _customerMessage;
        private static CancellationTokenSource _cancelationToken = new CancellationTokenSource();
        public async static Task MainAsync()
        {
            Console.WriteLine("Iniciando consumo da mensagem !");
            var queueUrlReponse = await _awsClient.GetQueueUrlAsync(_url);
            await ProcessQueueCustomerMessageAsync(queueUrlReponse);
        }

        #region Private Methods
        private async static Task ProcessQueueCustomerMessageAsync(GetQueueUrlResponse queueUrlReponse)
        {
            var receivedMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrlReponse.QueueUrl,
                // por default, aws não envia os atributos para poder ter mais eficiência no envio de mensagens
                AttributeNames = new List<string> { "All" },
                MessageAttributeNames = new List<string> { "All" }
            };

            while (!_cancelationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _awsClient.ReceiveMessageAsync(receivedMessageRequest, _cancelationToken.Token);
                    //if (response.Messages is null || response.Messages.Count == 0)
                    //    return;

                    foreach (var message in response.Messages)
                    {
                        Console.WriteLine($"Message Id: { message.MessageId }");
                        Console.WriteLine($"Message Body: { message.Body }");
                        // delete messages from queue
                        await _awsClient.DeleteMessageAsync(queueUrlReponse.QueueUrl, message.ReceiptHandle);
                    }
                    await Task.Delay(3000);
                    return;
                }
                catch (Exception ex)
                {
                    // Log errors
                    Console.WriteLine("Houve algum erro ! ");
                    throw;
                }
                
            }
        }
        #endregion
    }
}
