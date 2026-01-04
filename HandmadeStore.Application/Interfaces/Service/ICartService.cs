using HandmadeStore.Application.DTOs.cartdtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Service
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateCartAsync(string userId);
        Task<CartDto> GetCartAsync(string userId);
        Task AddItemAsync(string userId, int productId, int quantity);
        Task RemoveItemAsync(string userId, int productId);
        Task UpdateItemQuantityAsync(string userId, int productId, int quantity);
        Task ClearCartAsync(string userId);
    }
}
