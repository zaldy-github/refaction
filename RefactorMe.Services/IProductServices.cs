using System;
using System.Collections.Generic;
using RefactorMe.Entities;

namespace RefactorMe.Services
{
    public interface IProductServices
    {
        IEnumerable<ProductEntity> GetAllProducts();
        IEnumerable<ProductEntity> GetProductByName(string name);
        ProductEntity GetProductById(Guid productId);
        bool CreateProduct(ProductEntity productEntity);
        bool UpdateProduct(Guid productId, ProductEntity productEntity);
        bool DeleteProduct(Guid productId);
    }
}
