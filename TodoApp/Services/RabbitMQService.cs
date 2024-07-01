using RabbitMQ.Client;
using System.Text;

namespace TodoApp.Services
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory() {HostName = "rabbitMQ" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "todo_exchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue:"todo_queue",durable:true, exclusive:false,autoDelete:false,arguments:null);
            _channel.QueueBind(queue: "todo_queue", exchange: "todo_exchange", routingKey: "todo_key");
        }
        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange:"todo_exchange",routingKey:"todo_key",body:body,basicProperties:null);
        }
        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
