using EventBus.Abstractions;
using EventBus.Events;
using System;
using System.Collections.Generic;

namespace EventBus.Implementations
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly Dictionary<string, List<string>> _eventTypes;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new Dictionary<string, List<string>>();
        }

        public void AddSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent
        {
            var key = typeof(T).Name;
            if (!_handlers.ContainsKey(key))
            {
                _handlers[key] = new List<Type>();
            }

            if (!_handlers[key].Contains(typeof(TH)))
            {
                _handlers[key].Add(typeof(TH));
            }
        }

        public void RemoveSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent
        {
            var key = typeof(T).Name;
            if (_handlers.ContainsKey(key))
            {
                var handlerToRemove = _handlers[key].SingleOrDefault(h => h == typeof(TH));
                if (handlerToRemove != null)
                {
                    _handlers[key].Remove(handlerToRemove);
                }
            }
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = typeof(T).Name;
            return _handlers.ContainsKey(key) && _handlers[key].Any();
        }

        public void Clear()
        {
            _handlers.Clear();
        }
    }
}