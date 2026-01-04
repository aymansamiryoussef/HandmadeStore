    // Infrastructure/Services/OrderService.cs
    using AutoMapper;
    using HandmadeStore.Application.DTOs.OrderDtos;
    using HandmadeStore.Application.Interfaces;
    using HandmadeStore.Application.Interfaces.Repo;
    using HandmadeStore.Application.Interfaces.Service;
    using HandmadeStore.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace HandmadeStore.Infrastructure.Services
    {
        public class OrderService : IOrderService
        {
            private readonly IOrderRepo _orderRepo;
            private readonly ICartRepo _cartRepo;
            private readonly IGenericRepo<Product> _productRepo;
            private readonly IGenericRepo<Order> _genericOrderRepo;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public OrderService(
                IOrderRepo orderRepo,
                ICartRepo cartRepo,
                IGenericRepo<Product> productRepo,
                IGenericRepo<Order> genericOrderRepo,
                IUnitOfWork unitOfWork,
                IMapper mapper)
            {
                _orderRepo = orderRepo;
                _cartRepo = cartRepo;
                _productRepo = productRepo;
                _genericOrderRepo = genericOrderRepo;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }


        public async Task<OrderDto> CreateOrderFromCartAsync(string userId, CreateOrderDto createOrderDto)
        {

            var cart = await _cartRepo.GetActiveCartForUserAsync(userId);

            if (cart == null || !cart._items.Any())
                throw new Exception("Cart is empty");

            var order = new Order(
                userId,
                createOrderDto.ShippingAddress,
                createOrderDto.ShippingCity,
                createOrderDto.ShippingCountry,
                createOrderDto.ShippingZipCode,
                createOrderDto.PhoneNumber);

            var productIds = cart._items.Select(i => i.ProductId).ToList();
            var products = await _productRepo.GetAllAsync(p => productIds.Contains(p.Id));

            var productNames = products.ToDictionary(p => p.Id, p => p.Name);

            order.AddItemsFromCart(cart._items, productNames);


            order.SetShippingCost(CalculateShipping(order));
            order.SetTax(CalculateTax(order));

            await _genericOrderRepo.AddAsync(order);

            cart.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

                      await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, string userId)
            {
                var order = await _orderRepo.GetOrderWithItemsAsync(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                if (order.UserId != userId)
                    throw new Exception("Unauthorized access to order");

                return _mapper.Map<OrderDto>(order);
            }

            public async Task<OrderDto> GetOrderByNumberAsync(string orderNumber, string userId)
            {
                var order = await _orderRepo.GetOrderByNumberAsync(orderNumber);

                if (order == null)
                    throw new Exception("Order not found");

                if (order.UserId != userId)
                    throw new Exception("Unauthorized access to order");

                return _mapper.Map<OrderDto>(order);
            }

            public async Task<IReadOnlyList<OrderDto>> GetUserOrdersAsync(string userId)
            {
                var orders = await _orderRepo.GetUserOrdersAsync(userId);
                return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
            }

            public async Task ConfirmOrderAsync(int orderId, string userId)
            {
                var order = await _orderRepo.GetOrderByIdAsync(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                if (order.UserId != userId)
                    throw new Exception("Unauthorized");

                order.ConfirmOrder();
                _genericOrderRepo.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();
            }

            public async Task CancelOrderAsync(int orderId, string userId)
            {
                var order = await _orderRepo.GetOrderByIdAsync(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                if (order.UserId != userId)
                    throw new Exception("Unauthorized");

                order.CancelOrder();
                _genericOrderRepo.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();
            }

            public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
            {
                var order = await _orderRepo.GetOrderByIdAsync(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                switch (status)
                {
                    case OrderStatus.Confirmed:
                        order.ConfirmOrder();
                        break;
                    case OrderStatus.Processing:
                        order.MarkAsProcessing();
                        break;
                    case OrderStatus.Shipped:
                        order.MarkAsShipped();
                        break;
                    case OrderStatus.Delivered:
                        order.MarkAsDelivered();
                        break;
                    case OrderStatus.Cancelled:
                        order.CancelOrder();
                        break;
                }

                _genericOrderRepo.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();
            }

            public async Task MarkPaymentAsCompletedAsync(int orderId)
            {
                var order = await _orderRepo.GetOrderByIdAsync(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                order.MarkPaymentAsCompleted();
                order.ConfirmOrder();

                _genericOrderRepo.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();
            }

            // Helper methods for business logic
            private decimal CalculateShipping(Order order)
            {
                // Implement your shipping calculation logic
                // For example: based on weight, distance, or flat rate
                return 10.00m; // Flat rate for now
            }

            private decimal CalculateTax(Order order)
            {
                // Implement your tax calculation logic
                // For example: based on shipping location
                return order.SubTotal * 0.10m; // 10% tax for now
            }
        }
    }