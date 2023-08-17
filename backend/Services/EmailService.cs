using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ContRev.Backend;

public interface IEmailService
{
    Task<bool> Send( string to, string subject, string html );

}

public class EmailService : IEmailService {

    private readonly AppSettings _appSettings;

    public EmailService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task<bool> Send( string to, string subject, string html ) 
    {
        // create message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_appSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();
        smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
        smtp.Send(email);
        smtp.Disconnect(true);
        return true;
    }
}

public class EmailServiceMock : IEmailService {

    private readonly AppSettings _appSettings;

    public EmailServiceMock(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task<bool> Send( string to, string subject, string html ) 
    {
        Console.WriteLine("To: "+to);
        Console.WriteLine("Subject: "+subject);
        Console.WriteLine("Body: "+html);
        return true;
    }
}