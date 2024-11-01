using EventBus.Abstractions;
using EventBus.Events;
using InventoryService.Abstractions;

namespace InventoryService.EventHandlers
{
    public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
    {
        private readonly IInventoryService _inventoryService;

        public OrderCreatedEventHandler(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        public async Task Handle(OrderCreatedEvent @event)
        {
            if (@event.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }
            await _inventoryService.UpdateInventory(@event.ProductId, -@event.Quantity);
        }
    }

}
