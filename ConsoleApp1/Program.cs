using ConsoleApp1;
using RabbitMQ.Client;
using System.Text;
using System.Xml.Linq;

var mqConn = RabbitMQHelper.GetMQConnection();

//创建通道
using (var channel = mqConn.CreateModel())
{
    string exchangeName = "adminRouteExchange";//交换机名称
    //创建一个交换机(交换机的名字，交换机的类型) fanout 为订阅发布模式
    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    Console.WriteLine("交换机创建成功");
    string routeKey = "key2"; //匹配的key，
    //string queueName = "demo";
    ////声明一个队列
    //channel.QueueDeclare(
    //    queue: queueName, //队列名
    //    durable: false, //是否持久化，持久化可以让服务器重启后不丢失信息
    //    exclusive: false, //是否排他，为true，自己可见的队列，即不允许其它用户访问，只对首次声明它的连接（Connection）可见，会在其连接断开的时候自动删除
    //    autoDelete: false, //是否自动删除，前提是，至少有一个消费者连接到这个队列。当所有消费者都断开后才会自动删除
    //    arguments: null //设置队列的参数
    //);
    string str = string.Empty;
    do
    {
        Console.WriteLine("发送内容:");
        str = Console.ReadLine();
        //消息内容
        byte[] body = Encoding.UTF8.GetBytes(str);
        //发送消息
        channel.BasicPublish(exchangeName, routeKey, null, body);
        Console.WriteLine("成功发送消息:" + str);
    } while (str.Trim().ToLower() != "exit");
    mqConn.Close(); 
}
