using aws.credentials;
using aws.credentials.Models;
using aws.ses.send_email.Services;
using System.Text.Json;

namespace aws.ses.send_email
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Credentials.AddEnviromentVariable("AWS_DATA");

            Console.WriteLine("Sending email through smtp server ...");
            SendEmailThroughSmtpServer().GetAwaiter().GetResult();
            Console.WriteLine("Email successfully sent");

            Console.WriteLine("Sending email through aws sdk ...");
            SendEmailThroughAwsSdk().GetAwaiter().GetResult();
            Console.WriteLine("Email successfully sent");
        }

        /// <summary>
        /// Can't be tracked
        /// </summary>
        private static Task SendEmailThroughSmtpServer()
        {
            var awsData = JsonSerializer.Deserialize<AwsDataModel>(
                Environment.GetEnvironmentVariable("AWS_DATA"));

            var smtpService = new SmtpServerService(awsData);

            return smtpService.SendEmailAsync(
                "arthur151094@gmail.com",
                "My Subject SMTP SERVER",
                "My Content SMTP SERVER");
        }

        /// <summary>
        /// Can be tracked by MessageId
        /// </summary>
        private static async Task SendEmailThroughAwsSdk()
        {
            var awsData = JsonSerializer.Deserialize<AwsDataModel>(
                Environment.GetEnvironmentVariable("AWS_DATA"));

            var awsService = new AwsEmailService(
                awsData, Amazon.RegionEndpoint.USEast1);

            var response = await awsService.SendEmailAsync(
                "arthur151094@gmail.com",
                "My Subject AWS SDK",
                "My Content AWS SDK");

            Console.WriteLine("MessageId: " + response.MessageId);

            // Save the response in DB and use the project 'aws.sqs.bounce' to track
        }

    }
}
