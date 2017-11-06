using System;
using System.Collections.Generic;
using RefactorMe.Entities;

namespace RefactorMe.Services
{
    public interface IProductOptionServices
    {
        IEnumerable<ProductOptionEntity> GetOptionsForProduct(Guid productId);
        ProductOptionEntity GetByProductIdAndOptionId(Guid productId, Guid productOptionId);
        bool CreateProductOption(Guid productId, ProductOptionEntity productOptionEntity);
        bool UpdateProductOption(Guid productId, Guid productOptionId, ProductOptionEntity productOptionEntity);
        bool DeleteProductOption(Guid productId, Guid productOptionId);
    }
}
