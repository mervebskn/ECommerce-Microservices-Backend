using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Subscriptions
{
    public class Subscription
    {
        public string EventType { get; set; }
        public string Handler { get; set; }

        public Subscription(string eventType, string handler)
        {
            EventType = eventType;
            Handler = handler;
        }
    }

}
