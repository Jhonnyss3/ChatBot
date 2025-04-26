using Chat.Bot.API.DTOs;
using ChatBot.API.DTOs;
using ChatBot.Application.Chat;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("message")]
    public async Task<IActionResult> SendMessage([FromBody] UserMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Mensagem não pode ser vazia.");

        var (response, sessionId) = await _chatService.ProcessMessageAsync(request.Message, request.SessionId);
        return Ok(new ChatResponseDto { Response = response, SessionId = sessionId });
    }
}