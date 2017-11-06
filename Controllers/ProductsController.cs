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
    public class ProductsController : ApiController
    {
        private readonly IProductServices _productServices;

        // public constructor
        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>Product entities or Not Found</returns>
        [Route]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var products = _productServices.GetAllProducts();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        /// <summary>
        /// Find all products matching the specified name.
        /// </summary>
        /// <param name="name">
        /// Name of the product.
        /// </param>
        /// <returns>Product entity or Not Found</returns>
        [Route]
        [HttpGet]
        public IHttpActionResult Get([FromUri] string name)
        {
            var product = _productServices.GetProductByName(name);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Gets the product that matches the specified ID - ID is a GUID.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <returns>Product entity or Not Found</returns>
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var product = _productServices.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns>Ok or Bad Request</returns>
        [Route]
        [HttpPost]
        public IHttpActionResult Post([FromBody] ProductEntity productEntity)
        {
            if (!_productServices.CreateProduct(productEntity))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <param name="productEntity"></param>
        /// <returns>Ok or Bad Request</returns>
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Put(Guid id, [FromBody] ProductEntity productEntity)
        {
            if (id == Guid.Empty || !_productServices.UpdateProduct(id, productEntity))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Delete a product and its options.
        /// </summary>
        /// <param name="id">
        /// GUID of the product.
        /// </param>
        /// <returns>Ok or Bad Request</returns>
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            if (id == Guid.Empty || !_productServices.DeleteProduct(id))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
