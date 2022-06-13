using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;

using var sesClient = new AmazonSimpleEmailServiceClient();

var message = new MimeMessage
{
    Headers = { new Header("X-SES-CONFIGURATION-SET", "app-config-set") },
    From = { new MailboxAddress("App", "appemaxotech.link") },
    To = { new MailboxAddress("Elon Musk", "elon.muskêmaxotech.link") },
    Subject = "Test Email",
    Body = new BodyBuilder { TextBody = "This is a test message" }.ToMessageBody()
};

await using var messageStream = new MemoryStream();
await message.WriteToAsync(messageStream);

var request = new SendRawEmailRequest(new RawMessage { Data = messageStream });
await sesClient.SendRawEmailAsync(request);
