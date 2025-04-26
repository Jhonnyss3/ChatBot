using ChatBot.Domain.Entities;
using ChatBot.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ChatBot.Infrastructure.Services
{
    public class ChatSessionService : IChatSessionService
    {
        private readonly ConcurrentDictionary<string, ChatSessionContext> _sessions = new();

        public Task<ChatSessionContext> GetSessionAsync(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);
            return Task.FromResult(session ?? new ChatSessionContext { SessionId = sessionId });
        }

        public Task<ChatSessionContext> GetOrCreateSession(string sessionId)
        {
            var session = _sessions.GetOrAdd(sessionId, new ChatSessionContext { SessionId = sessionId });
            return Task.FromResult(session);
        }
    }
}
