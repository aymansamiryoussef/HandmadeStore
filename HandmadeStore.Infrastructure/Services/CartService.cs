    // Infrastructure/Services/CartService.cs
    using HandmadeStore.Application.DTOs.cartdtos;
    using HandmadeStore.Application.Interfaces;
    using HandmadeStore.Application.Interfaces.Repo;
    using HandmadeStore.Application.Interfaces.Service;
    using HandmadeStore.Domain.Entities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    namespace HandmadeStore.Infrastructure.Services
    {
        public class CartService : ICartService
        {
            private readonly ICartRepo _cartRepo;
            private readonly IGenericRepo<Product> _productRepo;
            private readonly IUnitOfWork _unitOfWork;

            public CartService(
                ICartRepo cartRepo,
                IGenericRepo<Product> productRepo,
                IUnitOfWork unitOfWork)
            {
                _cartRepo = cartRepo;
                _productRepo = productRepo;
                _unitOfWork = unitOfWork;
            }

            public async Task<Cart> GetOrCreateCartAsync(string userId)
            {
                var cart = await _cartRepo.GetActiveCartForUserAsync(userId);

                if (cart == null)
                {
                    cart = await _cartRepo.CreateCartAsync(userId);
                    await _unitOfWork.SaveChangesAsync();
                }

                return cart;
            }

            public async Task<CartDto> GetCartAsync(string userId)
            {
                var cart = await _cartRepo.GetActiveCartForUserAsync(userId);

                if (cart == null)
                    return null;

                // Get product details for cart items
                var productIds = cart._items.Select(i => i.ProductId).ToList();
                var products = await _productRepo.GetAllAsync(p => productIds.Contains(p.Id));

                var cartDto = new CartDto
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    Status = cart.Status.ToString(),
                    CreatedAt = cart.CreatedAt,
                    UpdatedAt = cart.UpdatedAt,
                    Items = cart._items.Select(item =>
                    {
                        var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                        return new CartItemDto
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            ProductName = product?.Name ?? "Unknown Product",
                            Price = item.Price,
                            Quantity = item.Quantity,
                            TotalPrice = item.Price * item.Quantity,
                            ProductImage = product?.MainImageUrl ?? ""
                        };
                    }).ToList()
                };

                cartDto.SubTotal = cartDto.Items.Sum(i => i.TotalPrice);
                cartDto.ItemCount = cartDto.Items.Sum(i => i.Quantity);

                return cartDto;
            }

            public async Task AddItemAsync(string userId, int productId, int quantity)
            {
                // Get or create cart
                var cart = await GetOrCreateCartAsync(userId);

                // Get product to validate and get price
                var product = await _productRepo.GetByIdAsync(productId);
                if (product == null)
                    throw new Exception("Product not found");

                // Add item to cart
                cart.AddItem(productId, product.Price, quantity);
                cart.UpdatedAt = DateTime.UtcNow;

                // Don't call UpdateAsync - cart is already tracked!
                // Cart is already tracked by EF Core, just save changes

                // Save changes
                await _unitOfWork.SaveChangesAsync();
            }

            public async Task RemoveItemAsync(string userId, int productId)
            {
                var cart = await _cartRepo.GetActiveCartForUserAsync(userId);
                if (cart == null)
                    throw new Exception("Cart not found");

                cart.RemoveItem(productId);
                cart.UpdatedAt = DateTime.UtcNow;

                // Don't call UpdateAsync - cart is already tracked!

                await _unitOfWork.SaveChangesAsync();
            }

            public async Task UpdateItemQuantityAsync(string userId, int productId, int quantity)
            {
                var cart = await _cartRepo.GetActiveCartForUserAsync(userId);
                if (cart == null)
                    throw new Exception("Cart not found");

                cart.ChangeQuantity(productId, quantity);
                cart.UpdatedAt = DateTime.UtcNow;

                // Don't call UpdateAsync - cart is already tracked!

                await _unitOfWork.SaveChangesAsync();
            }

            public async Task ClearCartAsync(string userId)
            {
                var cart = await _cartRepo.GetActiveCartForUserAsync(userId);
                if (cart == null)
                    throw new Exception("Cart not found");

                cart.Clear();
                cart.UpdatedAt = DateTime.UtcNow;

                // Don't call UpdateAsync - cart is already tracked!

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }