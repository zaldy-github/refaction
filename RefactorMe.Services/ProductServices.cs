using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using RefactorMe.Entities;
using RefactorMe.Models;
using RefactorMe.Models.UnitOfWork;

namespace RefactorMe.Services
{
    public class ProductServices : IProductServices
    {
        // private member variables
        private readonly UnitOfWork _unitOfWork;

        // public constructor
        public ProductServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get product entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetAllProducts()
        {
            var products = _unitOfWork.ProductRepository.GetAll().ToList();
            if (products.Any())
            {
                // map Product to ProductEntity
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Product, ProductEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var productEntities = mapper.Map<List<Product>, List<ProductEntity>>(products);
                return productEntities;
            }
            return null;
        }

        /// <summary>
        /// Get products by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<ProductEntity> GetProductByName(string name)
        {
            var product = _unitOfWork.ProductRepository.GetAll(p => p.Name == name).ToList();
            if (product.Any())
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Product, ProductEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var productEntities = mapper.Map<List<Product>, List<ProductEntity>>(product);
                return productEntities;
            }
            return null;
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductEntity GetProductById(Guid productId)
        {
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product != null)
            {
                // map Product to ProductEntity
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Product, ProductEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var productEntity = mapper.Map<Product, ProductEntity>(product);
                return productEntity;
            }
            return null;
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public bool CreateProduct(ProductEntity productEntity)
        {
            bool ok = false;
            try
            {
                using (var scope = new TransactionScope())
                {
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = productEntity.Name,
                        Description = productEntity.Description,
                        Price = productEntity.Price,
                        DeliveryPrice = productEntity.DeliveryPrice
                    };

                    _unitOfWork.ProductRepository.Insert(product);
                    _unitOfWork.Save();
                    scope.Complete();

                    ok = true;
                }
            }
            catch (Exception)
            {
            }

            return ok;
        }

        /// <summary>
        /// Update the product that matches the id specified
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public bool UpdateProduct(Guid productId, ProductEntity productEntity)
        {
            bool ok = false;
            if (productId != Guid.Empty)
            {
                if (productEntity != null)
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            var product = _unitOfWork.ProductRepository.GetById(productId);
                            if (product != null)
                            {
                                product.Name = productEntity.Name;
                                product.Description = productEntity.Description;
                                product.Price = productEntity.Price;
                                product.DeliveryPrice = productEntity.DeliveryPrice;

                                _unitOfWork.ProductRepository.Update(product);
                                _unitOfWork.Save();
                                scope.Complete();

                                ok = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return ok;
        }

        /// <summary>
        /// Delete the product that matches the id specfied
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool DeleteProduct(Guid productId)
        {
            bool ok = false;
            if (productId != Guid.Empty)
            {
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        var product = _unitOfWork.ProductRepository.GetById(productId);
                        if (product != null)
                        {
                            _unitOfWork.ProductRepository.Delete(product);
                            _unitOfWork.ProductOptionRepository.Delete(o => o.ProductId == productId);
                            _unitOfWork.Save();
                            scope.Complete();

                            ok = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return ok;
        }
    }
}
