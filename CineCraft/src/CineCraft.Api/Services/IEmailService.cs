namespace CineCraft.Api.Services;

public interface IEmailService
{
    Task SendApprovalEmail(string to, string userName);
    Task SendRejectionEmail(string to, string userName, string reason);
}
