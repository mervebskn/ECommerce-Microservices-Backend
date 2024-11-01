using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public class ProductCreatedEvent : IntegrationEvent
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }

        public ProductCreatedEvent(int productId, string productName, int stockQuantity)
        {
            ProductId = productId;
            ProductName = productName;
            StockQuantity = stockQuantity;
        }
    }

}
