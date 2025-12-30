using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System.Linq.Expressions;

namespace ECommerce.Services.Specifications.ProductSpecifications
{
    internal static class ProductSpecificationsHelper
    {
        public static Expression<Func<Product, bool>> GetProductCriteria(ProductQueryParamss queryParamss)
        {
            return p => (!queryParamss.BrandId.HasValue || p.BrandId == queryParamss.BrandId.Value)
             && (!queryParamss.TypeId.HasValue || p.TypeId == queryParamss.TypeId.Value)
             && (string.IsNullOrEmpty(queryParamss.Search) || p.Name.ToLower().Contains(queryParamss.Search.ToLower()));
        }
    }
}
