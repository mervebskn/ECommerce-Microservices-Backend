using Common.Models;

namespace InventoryService.Abstractions
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventories();
        Task<Inventory> GetInventoryById(int id);
        Task AddInventory(Inventory inventory);
        Task UpdateInventory(int productId, int quantityChange);
        Task DeleteInventory(int id);
    }
}
