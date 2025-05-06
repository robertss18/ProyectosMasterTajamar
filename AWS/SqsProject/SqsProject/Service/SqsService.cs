using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using SqsProject.Models;

namespace SqsProject.Service
{
    public class SqsService
    {

        private readonly AmazonSQSClient _sqsClient;
        private readonly string? _queueUrl;

        public SqsService(IOptions<AwsSettings> awsSettings)
        {
            _sqsClient = new AmazonSQSClient(
                awsSettings.Value.AccessKey,
                awsSettings.Value.SecretKey,
                RegionEndpoint.GetBySystemName(awsSettings.Value.Region)
            );

            _queueUrl = awsSettings.Value.QueueUrl;
        }

        //Enviar un mensaje a la cola
        public async Task<string> SendMessageAsync(string messageBody)
        {
            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = messageBody
            };

            var response = await _sqsClient.SendMessageAsync(request);
            return response.MessageId;

        }
        //Obtener mensajes de la cola
        public async Task<List<Message>> ReceiveMessageAsync()
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20
            };
            var response = await _sqsClient.ReceiveMessageAsync(request);
            return response.Messages;
        }
        //Eliminar un mensaje de la cola
        public async Task DeleteMessageAsync(string receiptHandle)
        {
            await _sqsClient.DeleteMessageAsync(_queueUrl, receiptHandle);
        }
        //Obtener el numero de mensajes en la cola no procesados
        public async Task<int> GetApproximateMessagesInQueueAsync()
        {
            var attributesRequest = new GetQueueAttributesRequest
            {
                QueueUrl = _queueUrl,
                AttributeNames = new List<string> { "ApproximateNumberOfMessages" }
            };

            var response = await _sqsClient.GetQueueAttributesAsync(attributesRequest);
            return int.Parse(response.Attributes["ApproximateNumberOfMessages"]);

        }
    }
}
