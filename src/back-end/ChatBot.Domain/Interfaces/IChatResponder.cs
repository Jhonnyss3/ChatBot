using System.Threading.Tasks;
using ChatBot.Domain.Models;

namespace ChatBot.Domain.Interfaces
{
    public interface IChatResponder
    {
        Task<string> RespondAsync(string message, string? sessionId = null);
        
    }
}
