using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR_Project.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalR_Project.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IHubContext<MyHub> _hubContext;

        public MessagesController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageToClients(string message)
        {
            await _hubContext.Clients.All.SendAsync("receiveMessage", message);
            return Ok();
        }
        
        
    }
}

