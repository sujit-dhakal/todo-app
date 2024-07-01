using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Data;
using TodoApp.Model;

namespace TodoApp.Services
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IEmailService _emailService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumerService(IEmailService emailService, IRabbitMQService rabbitMQService, IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitMQ" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "todo_exchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "todo_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "todo_queue", exchange: "todo_exchange", routingKey: "todo_key");
            _emailService = emailService;
            _rabbitMQService = rabbitMQService;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
                    var currentTime = DateTime.UtcNow;

                    var todo = await context.Todos.FirstOrDefaultAsync(t => t.Name == message);

                    if (todo != null && !todo.IsComplete && todo.CreatedAt.AddMinutes(1) < currentTime)
                    {
                        // Send message to another queue or service using RabbitMQ
                        _rabbitMQService.SendMessage($"Todo {todo.Name} is not completed");

                        // Send email notification
                        await _emailService.SendEmailAsync("recipient@example.com", "Todo Overdue", $"Todo {todo.Name} is not completed");
                    }
                }

                // Acknowledge the message (assuming autoAck is false)
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "todo_queue", autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
