using HandmadeStore.Application.Interfaces.Repo;
using HandmadeStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Infrastructure.Repositories
{
    public class CartRepo:GenericRepo<Cart>, ICartRepo
    {
        private readonly ApplicationDbContext _context;
        public CartRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<Cart> CreateCartAsync(string userId)
        {
            var cart = new Cart(userId);
            await _context.Carts.AddAsync(cart);
            return cart;
        }

        public async Task<Cart?> GetActiveCartForUserAsync(string userId)
        {
            return  await _context.Carts
           .Include(c => c._items)
           .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetCartWithItemsAsync(int cartId)
        {
            return await _context.Carts
             .Include(c => c._items)
             .FirstOrDefaultAsync(c => c.Id == cartId);
        }
    }
}
