using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Dtos;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext _dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseDto<ProductDto>>>> GetAllProducts()
        {
            var response = new ResponseDto<List<ProductDto>>();

            var dbProducts = await _dbContext.Products.ToListAsync();

            response.Result = mapper.Map<List<ProductDto>>(dbProducts);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetProductById(int id)
        {
            var response = new ResponseDto<ProductDto>();

            var dbProduct = await _dbContext.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();

            response.Result = mapper.Map<ProductDto>(dbProduct);

            return Ok(response);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetProductByCode(string categoryName)
        {
            var response = new ResponseDto<ProductDto>();

            var dbProduct = await _dbContext.Products.FirstAsync(x => x.CategoryName.ToLower() == categoryName.ToLower());

            if (dbProduct == null) return NotFound();

            response.Result = mapper.Map<ProductDto>(dbProduct);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<Product>>> CreateProduct(ProductDto model)
        {
            var response = new ResponseDto<Product>();

            var product = mapper.Map<Product>(model);

            await _dbContext.Products.AddAsync(product);

            await _dbContext.SaveChangesAsync();

            response.Result = product;

            return Created("Product added successfully", response);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<Product>>> UpdateProduct(ProductDto model)
        {
            var response = new ResponseDto<Product>();

            var product = mapper.Map<Product>(model);

            _dbContext.Update(product);

            await _dbContext.SaveChangesAsync();

            response.Result = product;

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<Product>>> DeleteProduct(int id)
        {
            var dbProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (dbProduct == null) return NotFound();

            var response = new ResponseDto<Product>();

            response.Result = null;

            _dbContext.Products.Remove(dbProduct);

            await _dbContext.SaveChangesAsync();

            return Ok(response);
        }
    }
}
