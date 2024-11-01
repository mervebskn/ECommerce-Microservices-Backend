using Common.DTOs;
using InventoryService.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    //[Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetAllInventories()
        {
            var inventories = await _inventoryService.GetAllInventories();
            return Ok(inventories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDto>> GetInventoryById(int id)
        {
            var inventory = await _inventoryService.GetInventoryById(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        [HttpPost]
        public async Task<ActionResult> AddInventory([FromBody] InventoryDto inventoryDto)
        {
            await _inventoryService.AddInventory(inventoryDto);
            return CreatedAtAction(nameof(GetInventoryById), new { id = inventoryDto.Id }, inventoryDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInventory(int id, [FromBody] InventoryDto inventoryDto)
        {
            await _inventoryService.UpdateInventory(id, inventoryDto.Quantity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            await _inventoryService.DeleteInventory(id);
            return NoContent();
        }
    }
}
