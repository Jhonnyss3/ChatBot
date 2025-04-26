using ChatBot.Domain.Interfaces;

namespace ChatBot.Infrastructure.ChatResponder
{
    public class BasicChatResponder : IChatResponder
    {
        public Task<string> RespondAsync(string message, string? sessionId = null)
        {
            return Task.FromResult($"Você disse: {message}");
        }
    }
}
