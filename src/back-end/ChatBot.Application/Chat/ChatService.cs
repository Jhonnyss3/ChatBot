using ChatBot.Domain.Interfaces;

namespace ChatBot.Application.Chat;

public class ChatService : IChatService
{
    private readonly IChatResponder _chatResponder;

    public ChatService(IChatResponder chatResponder)
    {
        _chatResponder = chatResponder;
    }

    public async Task<string> HandleMessageAsync(string message)
    {
        return await _chatResponder.RespondAsync(message);
    }
}

