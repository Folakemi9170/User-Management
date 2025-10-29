using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.DTO.ProductDto;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Entities.Product;
using UserManagement.Infrastructure;
using UserManagement.Utility;

namespace UserManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Products> _repo;
        private readonly IMapper _mapper;
        private readonly UMSDbContext _dbContext;
        private readonly AvailabilityMessage _availabilityMessage;

        public ProductService(IGenericRepository<Products> repo, IMapper mapper, UMSDbContext dbContext, AvailabilityMessage availabilityMessage)
        {
            _repo = repo;
            _mapper = mapper;
            _dbContext = dbContext;
            _availabilityMessage = availabilityMessage;
        }

        public async Task<ProductResponseDto> CreateProduct(CreateProduct product)
        {
            if (product.ProductCategoryId == null)
                throw new ArgumentException("Product category is required");

            var category = await _dbContext.Set<ProductCategory>()
                .FirstOrDefaultAsync(c => c.Id == product.ProductCategoryId);
            if (category == null)
                throw new ArgumentException("Input a valid product category");

            if (product.ProductStatusId == null)
                throw new ArgumentException("Product status is required");

            var status = await _dbContext.Set<ProductStatus>()
                .FirstOrDefaultAsync(c => c.Id == product.ProductStatusId);
            if (status == null)
                throw new ArgumentException("Input a valid product status");

            if (product.ProductTypeId == null)
                throw new ArgumentException("Product type is required");
            if (!await _dbContext.Set<ProductType>().AnyAsync(c => c.Id == product.ProductTypeId))
                throw new ArgumentException("Input a valid product type");

            string generatedCode = await CodeGenerator.GenerateProductCodeAsync(_dbContext, product.DepartmentId);

            var entity = _mapper.Map<Products>(product);
            entity.ProductCode = generatedCode;

            entity.AvailabilityMessage = await _availabilityMessage.GetAvailabilityMessage(entity);

            _dbContext.Product.Add(entity);
            await _dbContext.SaveChangesAsync();

            var createdProduct = await _repo.GetWithIncludesAsync(entity.Id, e => e.CreatedBy, e => e.AssignedTo, e => e.Department, e => e.ProductCategory, e => e.ProductType, e => e.Customer);

            var response = _mapper.Map<ProductResponseDto>(createdProduct);

            return response;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProducts()
        {
            var products = await _repo.GetAllWithIncludesAsync(p => p.Department,p => p.CreatedBy, p => p.AssignedTo, p => p.ProductCategory, p => p.ProductType, p => p.ProductStatus, p => p.Customer);

            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto> UpdateProduct(int id, UpdateProductDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("Product detail is required");

            bool hasChanges = !string.IsNullOrWhiteSpace(dto.ProductName) || !string.IsNullOrWhiteSpace(dto.ProductDescription) || (dto.Price != 0) || (dto.ProductStatusId != 0) || (dto.ProductTypeId != 0);

            if (!hasChanges)
                throw new InvalidOperationException("No valid fields provided to update.");

            var product = await _dbContext.Product.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product with Id not found");

            _mapper.Map(dto, product);

            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();

            var createdProduct = await _repo.GetWithIncludesAsync(id, p => p.Department, p => p.CreatedBy, p => p.AssignedTo, p => p.ProductCategory, p => p.ProductType, p => p.ProductStatus, p => p.Customer);

            return _mapper.Map<ProductResponseDto>(product);
        }
    }
}