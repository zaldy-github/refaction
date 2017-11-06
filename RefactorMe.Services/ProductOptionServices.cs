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
    public class ProductOptionServices : IProductOptionServices
    {
        // private member variables
        private readonly UnitOfWork _unitOfWork;

        // public constructor
        public ProductOptionServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get the product options for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<ProductOptionEntity> GetOptionsForProduct(Guid productId)
        {
            var productOptions = _unitOfWork.ProductOptionRepository.GetAll(o => o.ProductId == productId).ToList();
            if (productOptions.Any())
            {
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<ProductOption, ProductOptionEntity>();
                });
                IMapper mapper = config.CreateMapper();
                var productOptionEntities = mapper.Map<List<ProductOption>, List<ProductOptionEntity>>(productOptions);
                return productOptionEntities;
            }
            return null;
        }

        /// <summary>
        /// Get the product option that matches the product id and option id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <returns></returns>
        public ProductOptionEntity GetByProductIdAndOptionId(Guid productId, Guid productOptionId)
        {
            if (productId != Guid.Empty && productOptionId != Guid.Empty)
            {
                var productOption = _unitOfWork.ProductOptionRepository.Get(o => o.ProductId == productId && o.Id == productOptionId);
                if (productOption != null)
                {
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<ProductOption, ProductOptionEntity>();
                    });
                    IMapper mapper = config.CreateMapper();
                    var productOptionEntity = mapper.Map<ProductOption, ProductOptionEntity>(productOption);
                    return productOptionEntity;
                }
            }
            return null;
        }

        /// <summary>
        /// Create a product option for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionEntity"></param>
        /// <returns></returns>
        public bool CreateProductOption(Guid productId, ProductOptionEntity productOptionEntity)
        {
            bool ok = false;
            try
            {
                using (var scope = new TransactionScope())
                {
                    var productOption = new ProductOption
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        Name = productOptionEntity.Name,
                        Description = productOptionEntity.Description
                    };

                    _unitOfWork.ProductOptionRepository.Insert(productOption);
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
        /// Update the product option entity
        /// </summary>
        /// <param name="productOptionId"></param>
        /// <param name="productOptionEntity"></param>
        /// <returns></returns>
        public bool UpdateProductOption(Guid productId, Guid productOptionId, ProductOptionEntity productOptionEntity)
        {
            bool ok = false;
            if (productId != Guid.Empty && productOptionId != Guid.Empty)
            {
                if (productOptionEntity != null)
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            var productOption = _unitOfWork.ProductOptionRepository.Get(o => o.ProductId == productId && o.Id == productOptionId);
                            if (productOption != null)
                            {
                                productOption.Name = productOptionEntity.Name;
                                productOption.Description = productOptionEntity.Description;

                                _unitOfWork.ProductOptionRepository.Update(productOption);
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
        /// Delete product option
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <returns></returns>
        public bool DeleteProductOption(Guid productId, Guid productOptionId)
        {
            bool ok = false;
            if (productId != Guid.Empty && productOptionId != Guid.Empty)
            {
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        var productOption = _unitOfWork.ProductOptionRepository.Get(o => o.ProductId == productId && o.Id == productOptionId);
                        if (productOption != null)
                        {
                            _unitOfWork.ProductOptionRepository.Delete(productOption);
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
