using ECommerce.Shared;
using ECommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParamss queryParamss);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
    }
}
