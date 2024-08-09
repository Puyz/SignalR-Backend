using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR_Project.Context;
using SignalR_Project.Dtos;
using SignalR_Project.Hubs;
using SignalR_Project.Models;

namespace SignalR_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class ChatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        IHubContext<ChatHub> _hubContext;

        public ChatsController(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _context.Users.OrderBy(p => p.Name).ToListAsync();    
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetChats(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {
            List<Chat> chatList = await _context.Chats
                .Where(c => c.UserId == userId && c.ToUserId == toUserId || c.UserId == toUserId && c.ToUserId == userId)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);

            return Ok(chatList);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageDto messageDto, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = messageDto.UserId,
                ToUserId = messageDto.ToUserId,
                Message = messageDto.Message,
                Date = DateTime.Now

            };
            await _context.AddAsync(chat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            string connectionId = ChatHub.Users.First(p => p.Value == chat.ToUserId).Key;

            await _hubContext.Clients.Client(connectionId).SendAsync("messages", chat, cancellationToken);

            return Ok(chat);
        }
    }
}
