using RabbitMQ.Client;
using System.Text;

namespace Producers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            demo();
        }


        public static void demo()
        {
            // 连接工厂
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "admin",
                HostName = "localhost",
            };
            // 创建工厂
            var connection = factory.CreateConnection();
            // 创建通道
            var channel = connection.CreateModel();
            // 声明队列
            channel.QueueDeclare("hello", false, false, false, null);

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            string input;
            do
            {
                input = Console.ReadLine();
                var sendBytes = Encoding.UTF8.GetBytes(input);
                // 发布消息
                channel.BasicPublish("", "hello", null, sendBytes);

            } while (input.Trim().ToLower() != "exit");

            channel.Close();
            connection.Close();

        }
    }
}