using HandmadeStore.Application.DTOs;
using HandmadeStore.Application.DTOs.Product;
using HandmadeStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Service
{
    public interface IProductService
    {
        Task<IReadOnlyList<GetProductDto>> GetProductsAsync(ProductQueryParams query);
        Task<GetProductDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<GetProductDto>> GetAllActiveProductsAsync();

        Task<GetProductDto?> GetProductByNameAsync(string name);
        Task<GetProductDto> CreateAsync(CreateProductDto dto);
        Task UpdateAsync(int id, UpdateProductDto dto);
        Task DeleteAsync(int id);

    }
}
