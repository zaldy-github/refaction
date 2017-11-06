using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RefactorMe.Entities;
using RefactorMe.Services;

namespace RefactorMe.Controllers
{
    /*
     * There should be these endpoints:
     * GET /products - gets all products.
     * GET /products?name={name} - finds all products matching the specified name.
     * GET /products/{id} - gets the project that matches the specified ID - ID is a GUID.
     * POST /products - creates a new product.
     * PUT /products/{id} - updates a product.
     * DELETE /products/{id} - deletes a product and its options.
     * GET /products/{id}/options - finds all options for a specified product.
     * GET /products/{id}/options/{optionId} - finds the specified product option for the specified product.
     * POST /products/{id}/options - adds a new product option to the specified product.
     * PUT /products/{id}/options/{optionId} - updates the specified product option.
     * DELETE /products/{id}/options/{optionId} - deletes the specified product option.
     */

    [RoutePrefix("products")]
    public class ProductOptionsController : ApiController
    {
        private readonly IProductOptionServices _productOptionServices;

        // public constructor
        public ProductOptionsController(IProductOptionServices productOptionServices)
        {
            _productOptionServices = productOptionServices;
        }

        /// <summary>
        /// Find all options for a specified product.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <returns></returns>
        [Route("{id}/options")]
        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var productOptions = _productOptionServices.GetOptionsForProduct(id);
            if (productOptions == null)
            {
                return NotFound();
            }
            return Ok(productOptions);
        }

        /// <summary>
        /// Find the specified product option for the specified product.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <param name="optionId">
        /// GUID of the product option.
        /// </param>
        /// <returns></returns>
        [Route("{id}/options/{optionId}")]
        [HttpGet]
        public IHttpActionResult Get(Guid id, Guid optionId)
        {
            if (id == Guid.Empty || optionId == Guid.Empty)
            {
                return BadRequest();
            }

            var productOption = _productOptionServices.GetByProductIdAndOptionId(id, optionId);
            if (productOption == null)
            {
                return NotFound();
            }

            return Ok(productOption);
        }

        /// <summary>
        /// Add a new product option to the specified product.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <param name="productOptionEntity"></param>
        /// <returns></returns>
        [Route("{id}/options")]
        [HttpPost]
        public IHttpActionResult Post(Guid id, [FromBody] ProductOptionEntity productOptionEntity)
        {
            if (!_productOptionServices.CreateProductOption(id, productOptionEntity))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Update the specified product option.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <param name="optionId">
        /// GUID of the product option.
        /// </param>
        /// <param name="productOptionEntity"></param>
        /// <returns></returns>
        [Route("{id}/options/{optionId}")]
        [HttpPut]
        public IHttpActionResult Put(Guid id, Guid optionId, [FromBody] ProductOptionEntity productOptionEntity)
        {
            if (id == Guid.Empty ||
                optionId == Guid.Empty ||
                !_productOptionServices.UpdateProductOption(id, optionId, productOptionEntity))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Delete the specified product option.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <param name="optionId">
        /// GUID of the product option.
        /// </param>
        /// <returns></returns>
        [Route("{id}/options/{optionId}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id, Guid optionId)
        {
            if (id == Guid.Empty ||
                optionId == Guid.Empty ||
                !_productOptionServices.DeleteProductOption(id, optionId))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
