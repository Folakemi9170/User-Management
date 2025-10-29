using UserManagement.Application.DTO.ProductDto;

namespace UserManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProduct(CreateProduct dto);
        Task<ProductResponseDto> UpdateProduct(int id, UpdateProductDto dto);
        Task<IEnumerable<ProductResponseDto>> GetAllProducts();
        //Task<ProductResponseDto> GetProductByIdAsync(int id);

    }
}
