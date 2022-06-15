using aws.credentials.Models;
using System.Net.Mail;

namespace aws.ses.send_email.Services
{
    public class SmtpServerService : IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly AwsDataModel _awsData;

        public SmtpServerService(AwsDataModel awsData)
        {
            _smtpClient = new SmtpClient(awsData.Smpt.Host, awsData.Smpt.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(
                    awsData.Smpt.Username, awsData.Smpt.Password),
                EnableSsl = true
            };
            _awsData = awsData;

        }

        public Task SendEmailAsync(string toEmailAddress, string msgSubject,
            string msgBody, CancellationToken cancellationToken = new())
        {
            MailAddress fromAddress = new(_awsData.Ses.FromEmailAddress);
            MailAddress toAddress = new(toEmailAddress);

            MailMessage mailMsg = new(fromAddress, toAddress)
            {
                Subject = msgSubject,
                Body = msgBody,
            };

            if (!string.IsNullOrEmpty(_awsData.Ses.ConfigSet))
                mailMsg.Headers.Add("X-SES-CONFIGURATION-SET", _awsData.Ses.ConfigSet);

            return _smtpClient.SendMailAsync(mailMsg, cancellationToken);
        }


        public void Dispose() => _smtpClient.Dispose();

    }
}
