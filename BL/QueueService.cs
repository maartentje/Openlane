using System.Text;
using System.Text.Json;
using Domain;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BL;

public interface IQueueService
{
    void PostToQueue(Offer offer);
    void Listen();
}

public class QueueService : IQueueService
{
    private readonly IModel _channel;
    private readonly ILogger<QueueService> _logger;
    
    public QueueService(ILogger<QueueService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Connecting to RabbitMQ...");

        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq"
        };
        var conn = factory.CreateConnection();
        _channel = conn.CreateModel();
        _channel.QueueDeclare(queue: "default",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    public void PostToQueue(Offer offer)
    {
        var jsonString = JsonSerializer.Serialize(offer);
        var body = Encoding.UTF8.GetBytes(jsonString);
        _channel.BasicPublish(exchange: "",
            routingKey: "default",
            basicProperties: null,
            body: body);
        _logger.LogInformation(" ====> Published {json} to RabbitMQ", jsonString);
    }
    
    public void Listen()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation(" ====> Received {msg}", message);
            var offer = JsonSerializer.Deserialize<Offer>(message);
        };
        _channel.BasicConsume(queue: "default",
            autoAck: true,
            consumer: consumer);
    }
}