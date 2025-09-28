﻿using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductByCategoryAsync(string categoryName);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductsAsync(ProductDto ProductDto);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto ProductDto);
        Task<ResponseDto?> DeleteProductsAsync(int id);
    }
}