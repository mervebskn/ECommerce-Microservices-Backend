using System.Collections.Generic;

namespace EventBus.Subscriptions
{
    public class SubscriptionManager
    {
        private readonly Dictionary<string, List<string>> _subscriptions;

        public SubscriptionManager()
        {
            _subscriptions = new Dictionary<string, List<string>>();
        }

        public void AddSubscription(string eventType, string handler)
        {
            if (!_subscriptions.ContainsKey(eventType))
            {
                _subscriptions[eventType] = new List<string>();
            }

            if (!_subscriptions[eventType].Contains(handler))
            {
                _subscriptions[eventType].Add(handler);
            }
        }

        public List<string> GetHandlersForEvent(string eventType)
        {
            return _subscriptions.ContainsKey(eventType) ? _subscriptions[eventType] : new List<string>();
        }
    }
}
