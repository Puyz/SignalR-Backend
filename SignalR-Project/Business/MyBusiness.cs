using Microsoft.AspNetCore.SignalR;
using SignalR_Project.Hubs;

namespace SignalR_Project.Business;
public class MyBusiness
{
	private readonly IHubContext<MyHub> _hubContext;

    public MyBusiness(IHubContext<MyHub> hubContext)
    {
        _hubContext = hubContext;
    }


    public async Task SendMessageAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync("receiveMessage", message);
    }

}


