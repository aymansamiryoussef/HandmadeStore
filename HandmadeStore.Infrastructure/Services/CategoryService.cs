using AutoMapper;
using HandmadeStore.Application.DTOs.Category;
using HandmadeStore.Application.Interfaces;
using HandmadeStore.Application.Interfaces.Repo;
using HandmadeStore.Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 
        private readonly IGenericRepo<Category> _categoryRepo;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepo<Category> categoryRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            Category category = new Category(dto.Name, dto.Description);
            await _categoryRepo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);

        }

        public async Task DeleteAsync(int id)
        {
            var category = await  _categoryRepo.GetByIdAsync(id) 
                ?? throw new Exception("Category not found");
            
            category.deactivate();
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
        {
            var categories = await  _categoryRepo.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);

        }

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            var category =  _categoryRepo.GetByNameAsync(name);
            var CategoryDto=  _mapper.Map<CategoryDto>(category);
            return CategoryDto;

        }

        public async Task UpdateAsync(int id,CategoryDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            category.UpdateDetails(dto.Name, dto.Description);
            await _unitOfWork.SaveChangesAsync();
            

        }
    }
}
