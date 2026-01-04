using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Repo
{
    public interface ICartRepo
    {
        Task<Cart?> GetActiveCartForUserAsync(string userId);
        Task<Cart> CreateCartAsync(string userId);
        Task<Cart?> GetCartWithItemsAsync(int cartId);

    }
}
