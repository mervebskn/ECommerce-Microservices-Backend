using Common.Models;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.EntityFrameworkCore;
using OrderService.Abstractions;
using OrderService.Data;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _eventBus;

        public OrderService(IOrderRepository orderRepository, IEventBus eventBus)
        {
            _orderRepository = orderRepository;
            _eventBus = eventBus;
        }

        public async Task<Order> CreateOrderAsync(int productId, int quantity)
        {
            var order = new Order { ProductId = productId, Quantity = quantity, OrderDate = DateTime.UtcNow };
            await _orderRepository.AddAsync(order);

            _eventBus.Publish(new OrderCreatedEvent(order.Id, productId, quantity));

            return order;
        }

        public async Task<Order> UpdateOrderAsync(int orderId, int quantity)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Quantity = quantity;
            await _orderRepository.UpdateAsync(order);

            return order;
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.IsCanceled = true;
            await _orderRepository.UpdateAsync(order);

            //await _eventBus.Publish(new OrderCanceledEvent { OrderId = orderId });
        }
    }
}
