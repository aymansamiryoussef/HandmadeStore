using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
