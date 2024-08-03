using Microsoft.AspNetCore.SignalR;

namespace SignalR_Project.Hubs
{
    public class SalesHub : Hub
	{
		public async Task SendMessageAsync()
		{
			await Clients.All.SendAsync("receiveMessage", "Merhaba");
		}
	}
}

