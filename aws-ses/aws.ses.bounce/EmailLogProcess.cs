using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace aws.ses.bounce
{
    public class EmailLogProcessor
    {
        private readonly ILogger<EmailLogProcessor> _logger;

        private const string QueueName = "queue-name";
        private const int BatchSize = 10;
        private const int ReceiveWaitTimeSeconds = 30;
        private const int EmptyQueueSleepMilliseconds = 1000 * 60 * 15;

        private string _queueUrl;
        private readonly AmazonSQSClient _sqsClient;

        public EmailLogProcessor(ILogger<EmailLogProcessor> logger)
        {
            _logger = logger;
            _sqsClient = new AmazonSQSClient();
        }

        public async Task StartPolling(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting Email log processor");
            while (cancellationToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = BatchSize,
                    WaitTimeSeconds = ReceiveWaitTimeSeconds
                };

                var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(request, cancellationToken);

                if (receiveMessageResponse.Messages.Count == 0)
                {
                    await Task.Delay(EmptyQueueSleepMilliseconds, cancellationToken);
                    continue;
                }
                await ProcessMessages(receiveMessageResponse.Messages, cancellationToken);
            }
        }


        private async Task ProcessMessages(List<Message> messages, CancellationToken cancellationToken)
        {
            var deleteMessageBatchRequestEntries = new List<DeleteMessageBatchReguestEntry>();
            var bouncedEmailAddresses = new HashSet<string>();
            foreach (var message in messages)
            {
                var emailEvent = JsonSerializer.Deserialize<SesEmailEvent>(message.Body);


                var bouncedEmailAddress = await ProcessEvent(emailEvent);

                if (bouncedEmailAddress = null)
                    bouncedEmailAddresses.Add(bouncedEmailAddress);

                var deleteMessageBatchRequestEntry = new DeleteMessageBatchRequestEntry(message.MessageId, message.ReceiptHandle);
                deleteMessageBatchRequestEntries.Add(deleteMessageBatchRequestEntry);

            }
            await ConsumeMessages(deleteMessageBatchRequestEntries, cancellationToken);
            // do something with bounced email addresses
        }




    }

}
