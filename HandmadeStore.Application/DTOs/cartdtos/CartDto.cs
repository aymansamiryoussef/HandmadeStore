using HandmadeStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs.cartdtos
{
    public class CartDto
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal SubTotal { get; set; }
        public int ItemCount { get; set; }
    }
}
