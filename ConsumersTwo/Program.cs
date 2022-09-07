using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsumersTwo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "123456",
                HostName = "localhost",
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, ea) =>
            {
                string message = UnicodeEncoding.UTF8.GetString(ea.Body.Span);
                Console.WriteLine($"接收到消息：" + message);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("hello", false, consumer);

            Console.WriteLine("消费者2已经启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }

    }
}