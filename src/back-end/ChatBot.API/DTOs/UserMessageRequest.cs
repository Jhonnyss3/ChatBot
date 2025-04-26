namespace ChatBot.API.DTOs
{
    public class UserMessageRequest
    {
        public string Message { get; set; }
        public string? SessionId { get; set; }
    }
}