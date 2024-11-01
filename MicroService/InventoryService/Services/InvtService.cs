using AutoMapper;
using Common.DTOs;
using Common.Models;
using InventoryService.Abstractions;

namespace InventoryService.Services
{
    public class InvtService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public InvtService(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllInventories()
        {
            var inventories = await _inventoryRepository.GetAllInventories();
            return _mapper.Map<IEnumerable<InventoryDto>>(inventories);
        }

        public async Task<InventoryDto> GetInventoryById(int id)
        {
            var inventory = await _inventoryRepository.GetInventoryById(id);
            return _mapper.Map<InventoryDto>(inventory);
        }

        public async Task AddInventory(InventoryDto inventoryDto)
        {
            var inventory = _mapper.Map<Inventory>(inventoryDto);
            await _inventoryRepository.AddInventory(inventory);
        }
        public async Task UpdateInventory(int productId, int quantityChange)
        {
            var inventory = await _inventoryRepository.GetInventoryById(productId);

            if (inventory != null)
            {
                // Güncellenen miktarı gönderiyoruz.
                inventory.Quantity += quantityChange;

                // Eğer miktar negatif olmamalıysa, bu kontrolü ekleyebilirsiniz.
                if (inventory.Quantity < 0)
                {
                    throw new Exception("Inventory quantity cannot be less than zero.");
                }

                await _inventoryRepository.UpdateInventory(productId, inventory.Quantity);
            }
            else
            {
                throw new Exception("Inventory not found for the given product ID.");
            }
        }


        public async Task DeleteInventory(int id)
        {
            await _inventoryRepository.DeleteInventory(id);
        }
    }

}
