
using Microsoft.AspNetCore.SignalR;
using SignalR_Project.DataSources;
using SignalR_Project.Models;

namespace SignalR_Project.Hubs;

public class ChatHub : Hub
{
	public async Task GetUsername(string username)
	{
		Client client = new()
		{
			ConnectionId = Context.ConnectionId,
			Username = username
		};

		ClientDataSource.Clients.Add(client);

		await Clients.Others.SendAsync("clientJoined", username);
		await Clients.All.SendAsync("clients", ClientDataSource.Clients);
	}

	public async Task SendMessageAsync(string message, Client client)
	{
		var sender = ClientDataSource.Clients.FirstOrDefault(c => c.ConnectionId.Equals(Context.ConnectionId));
		if (client != null)
		{
			await Clients.Client(client.ConnectionId).SendAsync("receiveMessage", message, sender);
		}else
		{
			await Clients.Others.SendAsync("receiveMessage", message, sender);
		}
	}

    public async Task SendMessageToGroupAsync(string message, string groupName)
    {
        var sender = ClientDataSource.Clients.FirstOrDefault(c => c.ConnectionId.Equals(Context.ConnectionId));
        
        await Clients.Group(groupName).SendAsync("receiveMessage", message, sender);
        
    }

    public async Task AddGroup(string groupName)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

		Group group = new()
		{
			GroupName = groupName
		};
		group.Clients.Add(ClientDataSource.Clients.FirstOrDefault(c => c.ConnectionId.Equals(Context.ConnectionId))!);
		GroupDataSource.Groups.Add(group);

		await Clients.All.SendAsync("groups", GroupDataSource.Groups);
	}

	public async Task AddClientToGroup(string groupName)
	{
		Client client = ClientDataSource.Clients.FirstOrDefault(c => c.ConnectionId.Equals(Context.ConnectionId))!;

		Group group = GroupDataSource.Groups.FirstOrDefault(g => g.GroupName.Equals(groupName))!;

		if (!group.Clients.Any(c => c.ConnectionId.Equals(client.ConnectionId)))
		{
			group.Clients.Add(client);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);	
		}
		
	}

	public async Task GetClientsInGroup(string groupName)
	{
		var group = GroupDataSource.Groups.FirstOrDefault(g => g.GroupName.Equals(groupName));

		await Clients.Caller.SendAsync("clients", group.Clients);
	}

    public async Task GetAllClients()
	{ 
        await Clients.Caller.SendAsync("clients", ClientDataSource.Clients);
    }
}


