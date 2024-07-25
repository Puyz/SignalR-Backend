using Microsoft.AspNetCore.SignalR;

namespace SignalR_Project.Hubs;

public class MyHub : Hub
{
    private static List<string> clients = new(); // conntectionId Array

	public async Task SendMessageAsync(string message)
	{
		await Clients.All.SendAsync("receiveMessage", message);
	}

    public override async Task OnConnectedAsync()
    {
        clients.Add(Context.ConnectionId);
        await Clients.All.SendAsync("clients", clients);
        await Clients.All.SendAsync("userConnected", Context.ConnectionId);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Remove(Context.ConnectionId);
        await Clients.All.SendAsync("clients", clients);
        await Clients.All.SendAsync("userDisconnected", Context.ConnectionId);
    }
}


