using Common.Models;

namespace OrderService.Abstractions
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int productId, int quantity);
        Task<Order> UpdateOrderAsync(int orderId, int quantity);
        Task CancelOrderAsync(int orderId);
    }
}
