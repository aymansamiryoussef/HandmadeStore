using HandmadeStore.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Service
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderFromCartAsync(string userId, CreateOrderDto createOrderDto);
        Task<OrderDto> GetOrderByIdAsync(int orderId, string userId);
        Task<OrderDto> GetOrderByNumberAsync(string orderNumber, string userId);
        Task<IReadOnlyList<OrderDto>> GetUserOrdersAsync(string userId);
        Task ConfirmOrderAsync(int orderId, string userId);
        Task CancelOrderAsync(int orderId, string userId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task MarkPaymentAsCompletedAsync(int orderId);
    }
}
