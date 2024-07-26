using Microsoft.AspNetCore.SignalR;
using SignalR_Project.Interfaces;

namespace SignalR_Project.Hubs;

public class MyHub : Hub<IMessageClient>
{
    private static List<string> clients = new();

	//public async Task SendMessageAsync(string message)
	//{
	//	await Clients.All.SendAsync("receiveMessage", message);
	//}

    public override async Task OnConnectedAsync()
    {
        clients.Add(Context.ConnectionId);
        //await Clients.All.SendAsync("clients", clients);
        //await Clients.All.SendAsync("userConnected", Context.ConnectionId);
        await Clients.All.Clients(clients);
        await Clients.All.UserConnected(Context.ConnectionId);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Remove(Context.ConnectionId);
        //await Clients.All.SendAsync("clients", clients);
        //await Clients.All.SendAsync("userDisconnected", Context.ConnectionId);
        await Clients.All.Clients(clients);
        await Clients.All.UserDisconnected(Context.ConnectionId);
    }
}


