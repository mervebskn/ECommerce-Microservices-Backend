using Common.DTOs;
using Common.Models;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.Abstractions;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEventBus _eventBus;

        public OrdersController(IEventBus eventBus, IOrderService orderService)
        {
            _orderService = orderService;
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null || createOrderDto.Quantity <= 0)
            {
                return BadRequest("Invalid order data. Please ensure the product ID and quantity are valid.");
            }
            var order = await _orderService.CreateOrderAsync(createOrderDto.ProductId, createOrderDto.Quantity);

            if (order == null)
            {
                return StatusCode(500, "An error occurred while creating the order.");
            }
            var orderCreatedEvent = new OrderCreatedEvent(
                order.Id,
                createOrderDto.ProductId,
                createOrderDto.Quantity
            );
            _eventBus.Publish(orderCreatedEvent);

            return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var order = await _orderService.UpdateOrderAsync(id, updateOrderDto.Quantity);
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderService.CancelOrderAsync(id);
            return NoContent();
        }
    }

}
