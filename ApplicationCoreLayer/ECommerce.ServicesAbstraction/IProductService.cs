using ECommerce.Shared;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.ProductDTOs;

namespace ECommerce.ServicesAbstraction
{
    public interface IProductService
    {
        Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAsync(ProductQueryParamss queryParamss);
        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        Task<Result<IEnumerable<TypeDTO>>> GetAllTypesAsync();
        Task<Result<IEnumerable<BrandDTO>>> GetAllBrandsAsync();
    }
}
