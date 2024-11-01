using EventBus.Abstractions;
using EventBus.Connections;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Implementations
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subscriptionsManager;
        private readonly ILogger<EventBusRabbitMQ> _logger;

        public EventBusRabbitMQ(
            IRabbitMQPersistentConnection persistentConnection,
            IEventBusSubscriptionsManager subscriptionsManager,
            ILogger<EventBusRabbitMQ> logger)
        {
            _persistentConnection = persistentConnection;
            _subscriptionsManager = subscriptionsManager;
            _logger = logger;
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                throw new InvalidOperationException("RabbitMQ connection is not established.");
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

                channel.BasicPublish(exchange: "",
                                     routingKey: EventBusConstants.ProductCreatedEvent,
                                     basicProperties: null,
                                     body: body);
            }

            _logger.LogInformation($"Event published: {@event.GetType().Name}");
        }


        public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            // Subscribe logic here
            _subscriptionsManager.AddSubscription<T, TH>();
            _logger.LogInformation($"Subscribed to event: {typeof(T).Name}");
        }

        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            // Unsubscribe logic here
            _subscriptionsManager.RemoveSubscription<T, TH>();
            _logger.LogInformation($"Unsubscribed from event: {typeof(T).Name}");
        }
    }
}
