using System.Threading.Tasks;

namespace ddac_bookmate.Services
{
    public interface ISNSService
    {
        Task<bool> PublishMessageAsync(string message, string subject, string userEmail = null);
    }
} 