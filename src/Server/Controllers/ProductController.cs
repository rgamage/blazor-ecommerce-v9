﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var results = await _productService.GetProductsAsync();
            return Ok(results);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var results = await _productService.GetProductAsync(productId);
            return Ok(results);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var results = await _productService.GetProductsByCategoryAsync(categoryUrl);
            return Ok(results);
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> SearchProductsWithSearchtext(string searchText)
        {
            var results = await _productService.SearchProductsAsync(searchText);
            return Ok(results);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> SearchProductsWithSuggestionsSearchtext(string searchText)
        {
            var results = await _productService.GetProductSearchSuggestionsAsync(searchText);
            return Ok(results);
        }
    }
}
