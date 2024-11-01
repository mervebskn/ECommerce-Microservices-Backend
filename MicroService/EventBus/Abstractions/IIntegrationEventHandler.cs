using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent>
    {
        Task Handle(TIntegrationEvent integrationEvent);
    }

}
