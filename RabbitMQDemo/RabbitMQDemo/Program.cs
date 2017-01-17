using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };//创建代理服务器实例。注意：HostName为Rabbit Server所在服务器的ip或域名，如果服务装在本机，则为localhost，默认端口5672
            using (var connection = factory.CreateConnection())//创建socket连接
            {
                using (var channel = connection.CreateModel())//channel中包含几乎所有的api来供我们操作queue
                {
                    //声明queue
                    channel.QueueDeclare(queue: "hello",//队列名
                                         durable: false,//是否持久化
                                         exclusive: false,//排它性
                                         autoDelete: false,//一旦客户端连接断开则自动删除queue
                                         arguments: null);//如果安装了队列优先级插件则可以设置优先级

                    string message = "Hello World!";//待发送的消息
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",//exchange名称
                                         routingKey: "hello",//如果存在exchange,则消息被发送到名称为hello的queue的客户端
                                         basicProperties: null,
                                         body: body);//消息体
                    Console.WriteLine(" [x] Sent {0}", message);

                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }


        public void receiver()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",//指定发送消息的queue，和生产者的queue匹配
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                //注册接收事件，一旦创建连接就去拉取消息
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                     noAck: true,//和tcp协议的ack一样，为false则服务端必须在收到客户端的回执（ack）后才能删除本条消息
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
