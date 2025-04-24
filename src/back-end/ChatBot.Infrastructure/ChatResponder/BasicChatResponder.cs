using ChatBot.Domain.Interfaces;

namespace ChatBot.Infrastructure.ChatResponder;

public class BasicChatResponder : IChatResponder
{
    public Task<string> RespondAsync(string message)
    {
        if (message.ToLower().Contains("oi"))
            return Task.FromResult("Olá! Como posso te ajudar hoje?");
        else
            return Task.FromResult($"Você disse: {message}");
    }
}

