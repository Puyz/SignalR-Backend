using Microsoft.AspNetCore.SignalR;

namespace SignalR_Project.Hubs
{
    public class MessageHub : Hub
	{

        //public async Task SendMessageAsync(string message, IEnumerable<string> connectionIds)
        public async Task SendMessageAsync(string message, string groupName)
        //public async Task SendMessageAsync(string message, string groupName, IEnumerable<string> connectionIds)
        //public async Task SendMessageAsync(string message, IEnumerable<string> groupNames)
        {
            #region Client Türleri
            #region Caller
            // Sadece server'a bildirim gönderen client ile iletişim kurar. 
            // await Clients.Caller.SendAsync("receiveMessage",message);
            #endregion
            #region All
            // Server'a bağlı olan tüm client'lar ile iletişim kurar. 
            // await Clients.All.SendAsync("receiveMessage", message);
            #endregion
            #region Others
            // Sadece server'a bildirim gönderen client dışında server'a bağlı olan tüm client'lar  ile iletişim kurar. 
            // await Clients.Others.SendAsync("receiveMessage", message);
            #endregion
            #endregion

            #region Hub Clients Metotları
            #region AllExcept
            // Belirtilen client'lar hariç server'a bağlı tüm client'lara bildiride bulunur.
            // await Clients.AllExcept(connectionIds).SendAsync("receiveMessage", message);
            #endregion
            #region Client
            // Sadece belirli bir client'a bildiride bulunur.
            // await Clients.Client(connectionIds.First()).SendAsync("receiveMessage", message);
            #endregion
            #region Clients
            // Sadece belirli client'lara bildiride bulunur.
            // await Clients.Clients(connectionIds).SendAsync("receiveMessage", message);
            #endregion
            #region Group
            // Belirtilen gruptaki tüm client'lara bildiride bulunur.
            // Önce gruplar oluşturulmalı ve ardından client'lar gruplara subsrice olmalı.
            // await Clients.Group(groupName).SendAsync("receiveMessage", message);
            #endregion
            #region GroupExcept
            // Belirtilen gruptaki, belirtilen client'lar dışındaki tüm client'lara bildiride bulunur.
            // await Clients.GroupExcept(groupName,connectionIds).SendAsync("receiveMessage", message);
            #endregion
            #region Groups
            // Birden çok gruptaki client'lara bildiride bulunur.
            // await Clients.Groups(groupNames).SendAsync("receiveMessage", message);
            #endregion
            #region OthersInGroup
            // Bildiride bulunan client harici gruptaki tüm client'lara bildiride bulunur.
            await Clients.OthersInGroup(groupName).SendAsync("receiveMessage", message);
            #endregion
            #endregion
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("getConnectionId", Context.ConnectionId);
        }


        public async Task AddGroupAsync(string groupName, string connectionId)
        {
            // ConnectionId'yi belirtilen gruba atar. Eğer grup yoksa otomatik oluşturur ve connectionId'yi atar.
            await Groups.AddToGroupAsync(connectionId, groupName);
        }
    }
}

