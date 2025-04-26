namespace ChatBot.Domain.Entities
{
    public class ChatSessionContext
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string State { get; set; } = "inicio";
        public string LastTopic { get; set; } = string.Empty;
    }
}
