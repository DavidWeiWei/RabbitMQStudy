using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsumersOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建链接工厂
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "admin",
                HostName = "localhost",
            };
            // 创建链接
            var connection = factory.CreateConnection();
            // 创建通道
            var channel = connection.CreateModel();
            // 事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            // 接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                string message = UnicodeEncoding.UTF8.GetString(ea.Body.Span);
                Console.WriteLine($"接收到消息：" + message);
                Console.WriteLine("延迟10秒发送回执");
                Thread.Sleep(10 * 1000);
                // RabbitMQ采用消息应答机制，即消费者收到一个消息之后，需要发送一个应答，
                // 然后RabbitMQ才会将这个消息从队列中删除，如果消费者在消费过程中出现异常，
                // 断开连接切没有发送应答，那么RabbitMQ会将这个消息重新投递。
                // 确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine($"已经发送回执：${ea.DeliveryTag}");
            };

            //启动消费者 设置为手动应答消息
            channel.BasicConsume("hello", false, consumer);

            Console.WriteLine("消费者1已经启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }
    }
}