    using HandmadeStore.Application.Interfaces.Repo;
    using HandmadeStore.Infrastructure.Data;
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace HandmadeStore.Infrastructure.Repositories
    {
        // Infrastructure/Repositories/OrderRepo.cs
        public class OrderRepo : GenericRepo<Order>, IOrderRepo
        {
            private readonly ApplicationDbContext _context;

            public OrderRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
            {
                _context = applicationDbContext;
            }

            public async Task<Order?> GetOrderByIdAsync(int orderId)
            {
                return await _context.Orders.FindAsync(orderId);
            }

            public async Task<Order?> GetOrderWithItemsAsync(int orderId)
            {
                return await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == orderId);
            }

            public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
            {
                return await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
            }

            public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId)
            {
                return await _context.Orders
                    .Include(o => o.Items)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();
            }
        }
    }
