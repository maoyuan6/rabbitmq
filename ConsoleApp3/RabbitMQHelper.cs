using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ConsoleApp3
{
    public class RabbitMQHelper
    {
        //连接mq
        public static IConnection GetMQConnection()
        {
            var factory = new ConnectionFactory() { HostName = "192.168.1.101", UserName = "admin", Password = "123456", Port = 5672 };
            return factory.CreateConnection();  //返回连接
        }
    }
}
