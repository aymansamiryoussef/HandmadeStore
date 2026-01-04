    // API/Controllers/OrdersController.cs
  
    using HandmadeStore.Application.DTOs.OrderDtos;
    using HandmadeStore.Application.Interfaces.Service;
    using HandmadeStore.Domain.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    namespace HandmadeStore.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize] // Requires authentication
        public class OrdersController : ControllerBase
        {
            private readonly IOrderService _orderService;

            public OrdersController(IOrderService orderService)
            {
                _orderService = orderService;
            }

            /// <summary>
            /// Create a new order from the user's cart
            /// </summary>
            /// <param name="createOrderDto">Shipping and contact information</param>
            /// <returns>Created order details</returns>
            [HttpPost]
            [ProducesResponseType(typeof(OrderDto), 200)]
            [ProducesResponseType(400)]
            public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var order = await _orderService.CreateOrderFromCartAsync(userId, createOrderDto);

                    return Ok(new
                    {
                        message = "Order created successfully",
                        order = order
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Get all orders for the current user
            /// </summary>
            /// <returns>List of user's orders</returns>
            [HttpGet]
            [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), 200)]
            public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetMyOrders()
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var orders = await _orderService.GetUserOrdersAsync(userId);

                    return Ok(orders);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Get a specific order by ID
            /// </summary>
            /// <param name="id">Order ID</param>
            /// <returns>Order details</returns>
            [HttpGet("{id}")]
            [ProducesResponseType(typeof(OrderDto), 200)]
            [ProducesResponseType(404)]
            public async Task<ActionResult<OrderDto>> GetOrderById(int id)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var order = await _orderService.GetOrderByIdAsync(id, userId);

                    return Ok(order);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Get a specific order by order number
            /// </summary>
            /// <param name="orderNumber">Order number (e.g., ORD-20240104123456-ABC123)</param>
            /// <returns>Order details</returns>
            [HttpGet("by-number/{orderNumber}")]
            [ProducesResponseType(typeof(OrderDto), 200)]
            [ProducesResponseType(404)]
            public async Task<ActionResult<OrderDto>> GetOrderByNumber(string orderNumber)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var order = await _orderService.GetOrderByNumberAsync(orderNumber, userId);

                    return Ok(order);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Confirm a pending order
            /// </summary>
            /// <param name="id">Order ID</param>
            [HttpPost("{id}/confirm")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            public async Task<ActionResult> ConfirmOrder(int id)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await _orderService.ConfirmOrderAsync(id, userId);

                    return Ok(new { message = "Order confirmed successfully" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Cancel an order
            /// </summary>
            /// <param name="id">Order ID</param>
            [HttpPost("{id}/cancel")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            public async Task<ActionResult> CancelOrder(int id)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await _orderService.CancelOrderAsync(id, userId);

                    return Ok(new { message = "Order cancelled successfully" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            // ==================== ADMIN ENDPOINTS ====================

            /// <summary>
            /// Update order status (Admin only)
            /// </summary>
            /// <param name="id">Order ID</param>
            /// <param name="request">Status update request</param>
            [HttpPut("{id}/status")]
            [Authorize(Roles = "Admin")] // Only admins can update order status
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            [ProducesResponseType(403)]
            public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto request)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    await _orderService.UpdateOrderStatusAsync(id, request.Status);

                    return Ok(new { message = $"Order status updated to {request.Status}" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Mark order payment as completed (Admin/Payment Gateway only)
            /// </summary>
            /// <param name="id">Order ID</param>
            [HttpPost("{id}/payment/complete")]
            [Authorize(Roles = "Admin")] // Or use a special payment gateway role
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            public async Task<ActionResult> MarkPaymentCompleted(int id)
            {
                try
                {
                    await _orderService.MarkPaymentAsCompletedAsync(id);

                    return Ok(new { message = "Payment marked as completed and order confirmed" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
    }