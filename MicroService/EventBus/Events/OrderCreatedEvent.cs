using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public class OrderCreatedEvent : IntegrationEvent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderCreatedEvent(int orderId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

}
