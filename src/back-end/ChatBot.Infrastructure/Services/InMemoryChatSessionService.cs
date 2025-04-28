using ChatBot.Domain.Entities;
using ChatBot.Domain.Interfaces;
using System.Collections.Concurrent;

namespace ChatBot.Infrastructure.Services
{
    public class InMemoryChatSessionService : IChatSessionService
    {
        private static readonly Dictionary<string, ChatSessionContext> _sessions = new();

        public Task<ChatSessionContext> GetSessionAsync(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out var session))
            {
                session = new ChatSessionContext
                {
                    SessionId = sessionId,
                    State = "inicio"
                };
                _sessions[sessionId] = session;
            }
            return Task.FromResult(session);
        }

        public Task RemoveSessionAsync(string sessionId)
        {
            _sessions.Remove(sessionId);
            return Task.CompletedTask;
        }

        public Task<ChatSessionContext?> GetSessionIfExistsAsync(string sessionId)
        {
            if (_sessions.TryGetValue(sessionId, out var session))
                return Task.FromResult<ChatSessionContext?>(session);
            return Task.FromResult<ChatSessionContext?>(null);
        }
    }
}
