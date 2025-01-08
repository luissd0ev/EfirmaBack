using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace FirmaDocumento.Areas.FEA.Utility
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailSender : IEmailSender
    {
        private EmailSettings _emailSettings;
        private readonly IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).AddJsonFile("archivodos.json", optional: true, reloadOnChange: true).Build();

        public EmailSender()
        {
            _emailSettings = new EmailSettings
            {
                MailServer = configuration["EmailSettings:MailServer"],
                MailPort = int.Parse(configuration["EmailSettings:MailPort"]),
                Sender = configuration["EmailSettings:Sender"],
                Password = configuration["EmailSettings:Password"],
                SenderName = configuration["EmailSettings:SenderName"]
            };

        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await SendEmailAsync(email, subject, message, null);
        }

        public async Task SendEmailAsync(string email, string subject, string message, string emailsCC)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                string[] senders = email.Split(';');
                foreach (var mail in senders)
                    if (mail != "")
                        mimeMessage.To.Add(new MailboxAddress("Usuario", mail.Trim(' ')));
                if (emailsCC != null && !emailsCC.Equals(""))
                {
                    string[] mailsSenders = emailsCC.Split(';');
                    foreach(var mail in mailsSenders)
                        mimeMessage.Cc.Add(new MailboxAddress("Usuario", mail.Trim(' ')));
                }
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.CheckCertificateRevocation = false;
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Recipient address rejected: mynewdomain.com"))
                {
                    string correo = ex.Message.Split(':')[0].Split('<')[1].Replace('>',' ');
                    throw new InvalidOperationException("El Dominio mynewdomain.com, rechazo el correo: "+ correo);
                }
                   
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }

        public EmailSettings()
        {
        }

        public EmailSettings(string mailServer, int mailPort, string senderName, string sender, string password)
        {
            MailServer = mailServer;
            MailPort = mailPort;
            SenderName = senderName;
            Sender = sender;
            Password = password;
        }
    }
}
