using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SignalR_Project.Models;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalR_Project.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            // eğitim amaçlı olduğu için katmanlar oluşturulmadı.
            ConnectionFactory factory = new()
            {
                Uri = new Uri("amqps")
            };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            // [queue: kuyruk ismi]
            // [durable: dayanıklı(olası resetleme durumuna karşın mesajlar kalıcı hale gelsin mi)]
            // [exclusive: birden fazla kanalın bu kuyruğa bağlanıp bağlanmaması durumu]
            // [autoDelete: tüm mesajlar bitince otomatik kuyruğu silsin mi]
            channel.QueueDeclare("messageQueue", false, false, false);


            // kuyruğa mesajlar binary olarak göndermeliyiz. Eğer user gibi bir model(complex değer) göndereceksek serializer etmeliyiz.
            string serializeData = JsonSerializer.Serialize(user);
            byte[] data = Encoding.UTF8.GetBytes(serializeData);
            channel.BasicPublish("", "messageQueue", body: data);
            return Ok();
        }
    }
}

