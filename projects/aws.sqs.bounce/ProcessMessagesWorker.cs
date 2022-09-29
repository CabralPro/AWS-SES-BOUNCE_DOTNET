using Amazon;
using Amazon.SQS.Model;
using aws.credentials;
using aws.credentials.Models;
using aws.sqs.bounce.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace aws.sqs.bounce
{
    public class ProcessMessagesWorker : BackgroundService
    {
        private readonly AwsBounceService _awsBounce;
        private readonly ILogger<ProcessMessagesWorker> _logger;

        public ProcessMessagesWorker(ILogger<ProcessMessagesWorker> logger)
        {
            Credentials.AddEnviromentVariable("AWS_DATA");

            var awsData = JsonSerializer.Deserialize<AwsDataModel>(
                Environment.GetEnvironmentVariable("AWS_DATA"));

            _logger = logger;
            _awsBounce = new AwsBounceService(awsData, RegionEndpoint.GetBySystemName(awsData.AwsRegionEndpoint));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker aws ses bounce initialized");
            
            int awaitMinutes = 10;

            while (!stoppingToken.IsCancellationRequested)
            {
                await ConsumeMessagesAwsSqs(stoppingToken);

                _logger.LogInformation($"New search scheduled to {DateTime.Now.AddMinutes(awaitMinutes)}");
              
                await Task.Delay(TimeSpan.FromMinutes(awaitMinutes), stoppingToken);
            }
        }

        private async Task ConsumeMessagesAwsSqs(CancellationToken stoppingToken)
        {
            var deleteMessages = new List<Message>();
            ReceiveMessageResponse response;

            while (true)
            {
                response = await _awsBounce.ReceiveMessagesAsync(stoppingToken);

                if (!response.Messages.Any())
                    return;

                foreach (var message in response.Messages)
                {
                    // 'message.MessageId' is the sended message identificator 
                    // Save the message in DB and use 'aws.sqs.bounce' to track status
                    _logger.LogInformation("MessageId received: " + message.MessageId);
                    deleteMessages.Add(message);
                }

                var resp = await _awsBounce.DeleteMessagesAsync(deleteMessages, stoppingToken);

                if (resp.Successful.Count != response.Messages.Count)
                    return;
            }

        }
    }
}
