using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;
        void RemoveSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        void Clear();
    }

}
