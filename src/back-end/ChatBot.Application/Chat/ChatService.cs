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
        bool isNewSession = false;

        // Se não veio sessionId, crie um novo
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            isNewSession = true;
        }

        // Tente obter a sessão
        var session = await _sessionService.GetSessionIfExistsAsync(sessionId);

        // Se não existe (foi removida/encerrada), crie um novo sessionId e nova sessão
        if (session == null)
        {
            sessionId = Guid.NewGuid().ToString();
            isNewSession = true;
            session = await _sessionService.GetSessionAsync(sessionId);
        }

        var response = await _chatResponder.RespondAsync(message, sessionId);
        return (response, sessionId);
    }
}