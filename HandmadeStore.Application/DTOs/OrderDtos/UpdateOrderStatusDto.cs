// API/Controllers/OrdersController.cs
using System.ComponentModel.DataAnnotations;

namespace HandmadeStore.API.Controllers
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}