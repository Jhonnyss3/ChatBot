// ChatBot.Application/Chat/IChatService.cs
namespace ChatBot.Application.Chat
{
    public interface IChatService
    {
        Task<string> HandleMessageAsync(string message);
    }
}

