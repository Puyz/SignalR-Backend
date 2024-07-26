namespace SignalR_Project.Interfaces
{
    public interface IMessageClient
	{
		Task Clients(List<string> clients);
		Task UserConnected(string message);
        Task UserDisconnected(string message);
    }
}

