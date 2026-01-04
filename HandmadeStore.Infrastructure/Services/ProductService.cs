

using AutoMapper;
using HandmadeStore.Application.DTOs;
using HandmadeStore.Application.DTOs.Product;
using HandmadeStore.Application.Interfaces;
using HandmadeStore.Application.Interfaces.Repo;
using HandmadeStore.Application.Interfaces.Service;
using HandmadeStore.Application.Specifications;

namespace HandmadeStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepo<Product> _productRepo;

        public ProductService(
            IUnitOfWork unitOfWork,
            IGenericRepo<Product> productRepo,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        // ===================== CREATE =====================
        public async Task<GetProductDto> CreateAsync(CreateProductDto dto)
        {
            // Use factory method in domain
            var product = new Product (
                dto.Name,
                dto.Description,
                dto.Price,
                dto.Quantity,
                dto.ImageUrls,
                dto.Colors
            );

            await _productRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetProductDto>(product);
        }

        // ===================== DELETE =====================
        public async Task DeleteAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            _productRepo.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<GetProductDto?> GetProductByNameAsync(string name)
        {
            var product = await _productRepo.GetByNameAsync(name);
            if (product == null)
                return null;
            return _mapper.Map<GetProductDto>(product);
        }

        // ===================== GET ACTIVE =====================
        public async Task<IReadOnlyList<GetProductDto>> GetAllActiveProductsAsync()
        {
           
            var products= await _productRepo.GetAllAsync();
            return _mapper.Map<IReadOnlyList<GetProductDto>>(products);
        }

        // ===================== GET BY ID =====================
        public async Task<GetProductDto?> GetByIdAsync(int id)
        {
            var product= await _productRepo.GetByIdAsync(id);
            return _mapper.Map<GetProductDto>(product);
        }

        // ===================== GET WITH FILTER =====================
        public async Task<IReadOnlyList<GetProductDto>> GetProductsAsync(ProductQueryParams query)
        {
            var spec = new AvailableProductsSpec(query);
            var products= await _productRepo.GetAllWithSepcifications(spec);
            return _mapper.Map<IReadOnlyList<GetProductDto>>(products);

        }

        // ===================== UPDATE =====================
        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            product.UpdateProduct(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.Quantity,
                dto.Images,
                dto.Colors
            );

            _productRepo.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
