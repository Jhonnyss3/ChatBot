using ChatBot.Domain.Entities;

namespace ChatBot.Domain.Interfaces
{
    public interface IChatSessionService
    {
        // Este método deve obter a sessão ou criar uma nova se não existir
        Task<ChatSessionContext> GetSessionAsync(string sessionId);
        // Novo método para encerrar chat
        Task RemoveSessionAsync(string sessionId);

        Task<ChatSessionContext?> GetSessionIfExistsAsync(string sessionId);
    }
}