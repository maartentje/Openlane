using System.Text;
using System.Text.Json;
using Domain.Model;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace BL;

public interface IQueueService
{
    void PostToQueue(Offer offer);
    void Listen(CancellationToken ct);
}

public class QueueService : IQueueService
{
    private IModel _channel = null!;
    private readonly ILogger<QueueService> _logger;
    private readonly IOfferService _offerService;

    public QueueService(ILogger<QueueService> logger, IOfferService offerService)
    {
        _logger = logger;
        _offerService = offerService;

        _logger.LogInformation("Creating RabbitMQ connection...");
        try
        {
            CreateConnection(0);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not create RabbitMQ connection");
        }
    }

    private void CreateConnection(int retries = 0, CancellationToken ct = default)
    {
        try
        {
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
            _logger.LogInformation("RabbitMQ connection established");
        }
        catch (BrokerUnreachableException e)
        {
            _logger.LogWarning("Could not connect to RabbitMQ: {e}", e.Message);
            Task.Delay(1000 * retries, ct).Wait(ct);

            if (retries >= 10)
                throw;

            CreateConnection(retries + 1, ct);
        }
    }

    public void Listen(CancellationToken ct)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var offer = JsonSerializer.Deserialize<Offer>(message)!;
            _logger.LogInformation("[{id}] Received on queue", offer.Id);
            //The only reason to Wait() is to simulate 'long processing time'
            //This way, it won't process the next message when the first one is still busy
            //Easier to demo - logs show new process every ~5 seconds, but with multiple replicas it shows multiple process
            _offerService.ProcessOffer(offer, ct).Wait(ct);
        };
        _channel.BasicConsume(queue: "default",
            autoAck: true,
            consumer: consumer);
    }

    public void PostToQueue(Offer offer)
    {
        _logger.LogInformation("[{id}] Posting to queue", offer.Id);
        var jsonString = JsonSerializer.Serialize(offer);
        var body = Encoding.UTF8.GetBytes(jsonString);
        _channel.BasicPublish(exchange: "",
            routingKey: "default",
            basicProperties: null,
            body: body);
        _logger.LogInformation("[{id}] Posted to queue", offer.Id);
    }
}