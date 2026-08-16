using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CineCraft.Api.Services;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public SmtpEmailService(IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendApprovalEmail(string to, string userName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CineCraft", _settings.FromEmail));
        message.To.Add(new MailboxAddress(userName, to));
        message.Subject = "¡Tu cuenta en CineCraft ha sido aprobada!";

        var body = $@"
            <html>
            
            <body style='font-family: Arial, sans-serif;'>
                <h1 style='color: #2c3e50;'>Bienvenido a CineCraft</h1>
                <p>Hola <strong>{userName}</strong>,</p>
                <p>Tu solicitud de registro ha sido <strong style='color: #27ae60;'>aprobada</strong>.</p>
                <p>Ya puedes iniciar sesión en la plataforma.</p>
                <br/>
                <p>Saludos,<br/>El equipo de CineCraft</p>
            </body>
            </html>
        ";

        message.Body = new TextPart("html") { Text = body };

        await SendEmailAsync(message);
    }

    public async Task SendRejectionEmail(string to, string userName, string reason)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CineCraft", _settings.FromEmail));
        message.To.Add(new MailboxAddress(userName, to));
        message.Subject = "Solicitud de registro rechazada";

        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h1 style='color: #2c3e50;'>Estado de tu solicitud</h1>
                <p>Hola <strong>{userName}</strong>,</p>
                <p>Lamentamos informarte que tu solicitud de registro en CineCraft ha sido <strong style='color: #e74c3c;'>rechazada</strong>.</p>
                <p><strong>Motivo:</strong> {reason}</p>
                <br/>
                <p>Si crees que es un error, puedes contactar con el soporte.</p>
                <p>Saludos,<br/>El equipo de CineCraft</p>
            </body>
            </html>
        ";

        message.Body = new TextPart("html") { Text = body };

        await SendEmailAsync(message);
    }

    private async Task SendEmailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al enviar el correo electrónico.", ex);
        }
    }
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
}