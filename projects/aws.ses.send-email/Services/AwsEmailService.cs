using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using aws.credentials.Models;
using MimeKit;

namespace aws.ses.send_email.Services
{
    /// <summary>
    /// Documentation
    /// https://docs.aws.amazon.com/ses/latest/APIReference/API_Operations.html
    /// </summary>
    public class AwsEmailService
    {
        private readonly AmazonSimpleEmailServiceClient _sesClient;
        private readonly AwsDataModel _awsDataModel;

        public AwsEmailService(AwsDataModel awsData, RegionEndpoint region)
        {
            _sesClient = new AmazonSimpleEmailServiceClient(
                awsData.AwsKeys.AwsAccessKeyId, 
                awsData.AwsKeys.AwsSecretAccessKey, 
                region);

            _awsDataModel = awsData;
        }

        public async Task<SendRawEmailResponse> SendEmailAsync(string toEmailAddress, string msgSubject,
            string msgBody, CancellationToken cancellationToken = new())
        {
            var message = new MimeMessage
            {
                From = { new MailboxAddress("", _awsDataModel.Ses.FromEmailAddress) },
                To = { new MailboxAddress("", toEmailAddress) },
                Subject = msgSubject,
                Body = new BodyBuilder { TextBody = msgBody }.ToMessageBody()
            };

            if (!string.IsNullOrEmpty(_awsDataModel.Ses.ConfigSet))
                message.Headers.Add("X-SES-CONFIGURATION-SET", _awsDataModel.Ses.ConfigSet);

            using var messageStream = new MemoryStream();
            await message.WriteToAsync(messageStream, cancellationToken);

            var request = new SendRawEmailRequest(new RawMessage { Data = messageStream });
            return await _sesClient.SendRawEmailAsync(request, cancellationToken);
        }

    }
}