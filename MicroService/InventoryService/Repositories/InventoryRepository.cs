using Common.Models;
using InventoryService.Abstractions;
using InventoryService.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace InventoryService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventories()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<Inventory> GetInventoryById(int id)
        {
            return await _context.Inventories.FindAsync(id);
        }

        public async Task AddInventory(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateInventory(Inventory inventory)
        //{
        //    _context.Inventories.Update(inventory);
        //    await _context.SaveChangesAsync();
        //}
        public async Task UpdateInventory(int productId, int quantityChange)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
            if (inventory == null)
            {
                throw new Exception("Inventory not found for the given product.");
            }
            inventory.Quantity += quantityChange;
            if (inventory.Quantity < 0)
            {
                throw new Exception("Inventory quantity cannot be less than zero.");
            }
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
