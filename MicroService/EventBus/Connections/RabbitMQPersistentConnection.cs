using EventBus.Connections;
using RabbitMQ.Client;
using System;
namespace EventBus.Connections
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly object _lock = new object();

        public bool IsConnected => _connection != null && _connection.IsOpen;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool TryConnect()
        {
            lock (_lock)
            {
                if (IsConnected) return true;

                try
                {
                    _connection = _connectionFactory.CreateConnection();
                    _connection.ConnectionShutdown += (sender, ea) => { /* Handle connection shutdown */ };
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connection is available.");
            }
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
