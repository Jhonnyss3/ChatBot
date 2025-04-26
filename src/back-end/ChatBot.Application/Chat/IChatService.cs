namespace ChatBot.Application.Chat
{
    public interface IChatService
    {
        Task<(string response, string sessionId)> ProcessMessageAsync(string message, string? sessionId = null);
    }
}
