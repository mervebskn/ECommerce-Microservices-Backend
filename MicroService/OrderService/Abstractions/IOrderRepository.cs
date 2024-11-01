using Common.Models;

namespace OrderService.Abstractions
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int orderId);
    }

}
