using Common.DTOs;

namespace InventoryService.Abstractions
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventories();
        Task<InventoryDto> GetInventoryById(int id);
        Task AddInventory(InventoryDto inventoryDto);
        Task UpdateInventory(int productId, int quantityChange);
        Task DeleteInventory(int id);
    }
}
