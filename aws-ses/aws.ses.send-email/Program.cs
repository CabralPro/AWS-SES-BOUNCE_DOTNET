using System.Net.Mail;

namespace aws.ses.send_email
{
    public class Program
    {
        static readonly string host = "email-smtp.us-east-1.amazonaws.com";
        static readonly string smtpUsername = "AKIAQDGGB72YCY7E7ZUD";
        static readonly string smtpPassword = "BDCxmd1L3IXKd54piKEpyxDu+tB/9h0JBdT7ANbGcgsi";
        static readonly int port = 587;

        static readonly string fromEmailAddress = "arthur151094@gmail.com";
        static readonly string toEmailAddress = "arthur151094@gmail.com";

        static readonly string msgSubject = "My Subject";
        static readonly string msgBody = "My content";

        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            SmtpClient client = new(host, port)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            MailAddress fromAddress = new(fromEmailAddress);
            MailAddress toAddress = new(toEmailAddress);

            MailMessage mailMsg = new(fromAddress, toAddress)
            {
                Subject = msgSubject,
                Body = msgBody
            };

            try
            {
                await client.SendMailAsync(mailMsg);
                Console.WriteLine("Email enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar email: " + ex.Message);
            }

            Console.ReadLine();
        }

    }
}
