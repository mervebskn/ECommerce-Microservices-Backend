using Common.Models;
using EventBus.Abstractions;
using EventBus.Events;
using InventoryService.Abstractions;

namespace InventoryService.EventHandlers
{
    public class ProductCreatedEventHandler : IIntegrationEventHandler<ProductCreatedEvent>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public ProductCreatedEventHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task Handle(ProductCreatedEvent @event)
        {
            //stok ekleme işlemi
            var inventoryItem = new Inventory
            {
                ProductId = @event.ProductId,
                ProductName = @event.ProductName,
                Quantity = @event.StockQuantity
            };

            await _inventoryRepository.AddInventory(inventoryItem);
        }
    }
}
