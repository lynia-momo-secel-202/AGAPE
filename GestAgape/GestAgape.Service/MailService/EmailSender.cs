using GestAgape.Service.MailService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.UserName, _emailSettings.UserName));
        message.To.Add(new MailboxAddress("Gomseu", email));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = htmlMessage
        };

        using (var client = new SmtpClient())
        {
            client.Connect(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
            client.Authenticate(_emailSettings.UserName, _emailSettings.Password);
            client.Send(message);
            client.Disconnect(true);
        }

        return Task.CompletedTask;
    }
}
