using ChatBot.Application.Chat;
using ChatBot.Domain.Interfaces;

public class ChatService : IChatService
{
    private readonly IChatResponder _chatResponder;
    private readonly IChatSessionService _sessionService;

    public ChatService(IChatResponder chatResponder, IChatSessionService sessionService)
    {
        _chatResponder = chatResponder;
        _sessionService = sessionService;
    }

    public async Task<(string response, string sessionId)> ProcessMessageAsync(string message, string? sessionId = null)
    {
        // Se não veio sessionId, crie uma nova sessão
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            //await _sessionService.CreateSessionAsync(sessionId); // Implemente esse método se necessário
        }

        var response = await _chatResponder.RespondAsync(message, sessionId);
        return (response, sessionId);
    }
}