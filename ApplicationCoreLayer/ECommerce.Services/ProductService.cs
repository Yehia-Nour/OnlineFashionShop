using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Specifications;
using ECommerce.Services.Specifications.ProductSpecifications;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.ProductDTOs;
using Error = ECommerce.Shared.CommonResult.Error;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParamss queryParamss)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var spec = new ProductWithTypeAndBrandSpecification(queryParamss);
            var products = await repo.GetAllAsync(spec);
            var dataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var countOfReturnData = dataToReturn.Count();
            var countSpec = new ProductCountSpecifications(queryParamss);
            var countOfAllProducts = await repo.CountAsync(countSpec);

            return new PaginatedResult<ProductDTO>(queryParamss.PageIndex, countOfReturnData, countOfAllProducts, dataToReturn);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithTypeAndBrandSpecification(id);

            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id: {id} Not Found");

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            return _mapper.Map<IEnumerable<TypeDTO>>(types);
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            return _mapper.Map<IEnumerable<BrandDTO>>(brands);
        }
    }
}
