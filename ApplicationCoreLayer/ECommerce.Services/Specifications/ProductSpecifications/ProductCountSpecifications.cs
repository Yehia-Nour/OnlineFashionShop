using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;

namespace ECommerce.Services.Specifications.ProductSpecifications
{
    internal class ProductCountSpecifications : BaseSpecification<Product, int>
    {
        public ProductCountSpecifications(ProductQueryParamss queryParamss)
             : base(ProductSpecificationsHelper.GetProductCriteria(queryParamss)) { }
    }
}
