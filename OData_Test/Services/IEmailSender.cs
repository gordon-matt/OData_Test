using System.Threading.Tasks;

namespace OData_Test.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}