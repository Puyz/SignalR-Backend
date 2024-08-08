using Microsoft.AspNetCore.SignalR;
using SignalR_Project.Context;
using SignalR_Project.Models;

namespace SignalR_Project.Hubs
{
    public sealed class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        public static Dictionary<string, Guid> Users = new();

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task Connect(Guid userId)
        {
            User user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                Users.Add(Context.ConnectionId, userId);
                user.Status = "online";
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("users", user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Users.TryGetValue(Context.ConnectionId, out Guid userId);

            User user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                Users.Remove(Context.ConnectionId);
                user.Status = "offline";
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("users", user);
            }
        }
    }
}
