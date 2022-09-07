using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsumersOne
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
                Console.WriteLine("延迟10秒发送回执");
                Thread.Sleep(10 * 1000);
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine($"已经发送回执：${ea.DeliveryTag}");
            };

            channel.BasicConsume("hello", false, consumer);

            Console.WriteLine("消费者1已经启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }
    }
}