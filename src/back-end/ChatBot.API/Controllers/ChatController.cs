using Chat.Bot.API.DTOs;
using ChatBot.Application.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserMessageRequest request)
    {
        var response = await _chatService.HandleMessageAsync(request.Message);
        return Ok(new ChatResponseDto { Response = response });
    }
}
