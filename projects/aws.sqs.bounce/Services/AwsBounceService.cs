using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon;
using aws.credentials.Models;

namespace aws.sqs.bounce.Services
{
    public class AwsBounceService
    {
        private readonly AmazonSQSClient _sqsClient;
        private readonly ReceiveMessageRequest _receiveMessageRequest;
        private readonly DeleteMessageBatchRequest _deleteMessageRequest;

        public AwsBounceService(AwsDataModel awsData, RegionEndpoint region)
        {
            _sqsClient = new AmazonSQSClient(
                awsData.AwsKeys.AwsAccessKeyId,
                awsData.AwsKeys.AwsSecretAccessKey,
                region);

            _receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = awsData.Sqs.Url,
                MaxNumberOfMessages = awsData.Sqs.MaxNumberOfMessages,
                WaitTimeSeconds = awsData.Sqs.WaitTimeSeconds
            };

            _deleteMessageRequest = new DeleteMessageBatchRequest
            {
                QueueUrl = awsData.Sqs.Url
            };
        }


        public Task<ReceiveMessageResponse> ReceiveMessagesAsync(CancellationToken stoppingToken) =>
            _sqsClient.ReceiveMessageAsync(_receiveMessageRequest, stoppingToken);

        public Task<DeleteMessageBatchResponse> DeleteMessagesAsync(List<Message> messages, CancellationToken stoppingToken) 
        {
            _deleteMessageRequest.Entries.Clear();

            foreach (var message in messages)
            {
                var deleteMessage = new DeleteMessageBatchRequestEntry(
                    message.MessageId, message.ReceiptHandle);

                _deleteMessageRequest.Entries.Add(deleteMessage);
            }

            return _sqsClient.DeleteMessageBatchAsync(_deleteMessageRequest, stoppingToken);
        }

    }

}
