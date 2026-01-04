using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Repo
{
    public interface IOrderRepo
    {
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<Order?> GetOrderByNumberAsync(string orderNumber);
        Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId);
    }
}
