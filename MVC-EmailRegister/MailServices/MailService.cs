
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC_EmailRegister.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MVC_EmailRegister.MailServices
{
    public class MailService : IMailService
    {
        private readonly SmtpSettings _smtpSettings;

        public MailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendMailAsync(string email, string subject, string message)
        {
            try
            {
                var newEmail = new MimeMessage();
                newEmail.From.Add(MailboxAddress.Parse("bilgeadam2024@gmail.com"));//hangi mail adresini kullancağımı söyledik.
                newEmail.To.Add(MailboxAddress.Parse(email)); //nereye gideceüini belirtiyoruz.
                newEmail.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = message;
                newEmail.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

                    await client.SendAsync(newEmail);
                    await client.DisconnectAsync(true);
                }
                //SMTP = MAİL GÖNDERME SUNUCUSUDUR. SİMPLE MAİL TRANSFER PROTOKOL
                //var smtp = new SmtpClient();
                //await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //await smtp.AuthenticateAsync("bilgeadam2024@gmail.com", "soubzozkdbkgyxpb");
                //await smtp.SendAsync(newEmail);
                //await smtp.DisconnectAsync(true);

            }
            catch (Exception ex)
            {

                throw new InvalidOperationException($"Eposta gönderilirken bir hata oluştu : {ex.Message}");
            }
        }
    }
}
