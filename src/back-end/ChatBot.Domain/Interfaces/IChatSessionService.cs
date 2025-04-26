using ChatBot.Domain.Entities;

namespace ChatBot.Domain.Interfaces
{
    public interface IChatSessionService
    {
        // Este método deve obter a sessão ou criar uma nova se não existir
        Task<ChatSessionContext> GetSessionAsync(string sessionId);
    }
}