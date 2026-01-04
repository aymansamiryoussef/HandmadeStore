using AutoMapper;
using HandmadeStore.Application.DTOs.cartdtos;
using HandmadeStore.Application.DTOs.OrderDtos;
using HandmadeStore.Application.DTOs.Product;
using HandmadeStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductDto>()
                .ForMember(d => d.ImageUrls,
                    o => o.MapFrom(s => s.Images.Select(i => i.ImageUrl)))
                .ForMember(d => d.Colors,
                    o => o.MapFrom(s => s.Colors.Select(c => c.Name)))
                .ForMember(d => d.IsSoldOut,
                    o => o.MapFrom(s => s.StockQuantity == 0));
            CreateMap<UpdateProductDto, Product>();
            CreateMap<CreateProductDto, Product>();

            CreateMap<CartItem, CartItemDto>();

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<Order, OrderDto>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
           .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));

            CreateMap<OrderItem, OrderItemDto>();

        }

    }
}
