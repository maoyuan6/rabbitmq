 
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Xml.Linq;
using ConsoleApp3;

Console.WriteLine("我是消费者2，我接收到了以下信息");

var mqConn = RabbitMQHelper.GetMQConnection();

//创建通道
using (var channel = mqConn.CreateModel())
{
    string exchangeName = "adminRouteExchange";//交换机名称
    //创建一个交换机(交换机的名字，交换机的类型) fanout 为订阅发布模式
    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    Console.WriteLine("交换机创建成功");
    //消息队列名称
    string queueName = Guid.NewGuid().ToString();
    //声明一个队列
    channel.QueueDeclare(
        queue: queueName, //队列名
        durable: false, //是否持久化，持久化可以让服务器重启后不丢失信息
        exclusive: false, //是否排他，为true，自己可见的队列，即不允许其它用户访问，只对首次声明它的连接（Connection）可见，会在其连接断开的时候自动删除
        autoDelete: false, //是否自动删除，前提是，至少有一个消费者连接到这个队列。当所有消费者都断开后才会自动删除
        arguments: null //设置队列的参数
    );

    string routeKey = "key2"; //匹配的key
    //将队列与交换机进行绑定
    channel.QueueBind(queueName, exchangeName, routeKey, null);
    //每次只能向消费者发送5条信息,再消费者未确认之前,不再向他发送信息
    channel.BasicQos(0, 5, false);
    //创建消费者对象
    var consumer = new EventingBasicConsumer(channel);
    Console.WriteLine($"队列名称:{queueName}");
    consumer.Received += (model, ea) =>
    {
        //接收到的消息
        byte[] message = ea.Body.ToArray();
        Console.WriteLine("接收到消息为:" + Encoding.UTF8.GetString(message));
        //Thread.Sleep(5000);
        channel.BasicAck(ea.DeliveryTag, true); //消息确认
    };//消费者开启监听
    channel.BasicConsume(queueName, false, consumer);//autoAck 自动确认关闭
    Console.ReadKey();
    //清除队列
    channel.Dispose();
    //链接关闭
    mqConn.Close();
}