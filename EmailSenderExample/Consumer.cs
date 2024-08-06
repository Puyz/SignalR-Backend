﻿using EmailSenderExample.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmailSenderExample
{
    public class Consumer
    {
        private readonly ConnectionFactory factory;
        public Consumer()
        {
            factory = new ConnectionFactory
            {
                Uri = new Uri("amqps")
            };
        }


        public void ConsumeQueue()
        {
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.QueueDeclare("messageQueue", false, false, false);

            EventingBasicConsumer consumer = new(channel);

            // [autoAck: kuyruktan alınan mesajın silinip silinmeme durumu]
            channel.BasicConsume("messageQueue", true, consumer);

            //consumer.Received += Consumer_Received;
            consumer.Received += (s, e) =>
            {
                // mail işlemleri burada gerçekleştirilecek.

                // [e.Body.Span: mesaja byte türünde erişiyoruz. ]

                // Öncelikle string türüne dönüştürebiliriz.
                string encodingData = Encoding.UTF8.GetString(e.Body.Span);
                User user = JsonSerializer.Deserialize<User>(encodingData)!;

                //EmailSender.SendMail(user.Email, user.Message);
                Console.WriteLine($"{user.Email}: mail gönderildi.");

            };
        }

        private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}