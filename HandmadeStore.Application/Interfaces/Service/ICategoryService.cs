using HandmadeStore.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Service
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(CategoryDto dto);
        Task UpdateAsync(int id,CategoryDto dto);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto?> GetByNameAsync(string name);

    }
}
