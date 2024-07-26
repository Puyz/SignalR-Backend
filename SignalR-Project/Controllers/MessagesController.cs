using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR_Project.Business;
using SignalR_Project.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalR_Project.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly MyBusiness _myBusiness;

        public MessagesController(MyBusiness myBusiness)
        {
            _myBusiness = myBusiness;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageToClients(string message)
        {
            await _myBusiness.SendMessageAsync(message);
            return Ok();
        }
        
        
    }
}

