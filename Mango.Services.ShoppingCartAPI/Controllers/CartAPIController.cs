using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Dtos;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController(IMapper _mapper, AppDbContext _context) : ControllerBase
    {
        private readonly ResponseDto _response = new();

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> Upsert(CartDto model)
        {
            try
            {
                CartHeader cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == model.CartHeader.UserId);

                if (cartHeader is null)
                {
                    // create cart header
                    var cartHeaderMap = _mapper.Map<CartHeader>(model.CartHeader);
                    await _context.CartHeaders.AddAsync(cartHeaderMap);
                    await _context.SaveChangesAsync();

                    model.CartDetails.First().CartHeaderId = cartHeaderMap.CartHeaderId;
                    await _context.CartDetails.AddAsync(_mapper.Map<CartDetail>(model.CartDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // check for duplicate products
                    CartDetail cartDetail = await _context.CartDetails.AsNoTracking().
                            FirstOrDefaultAsync(x => x.ProductId == model.CartDetails.First().ProductId && x.CartHeaderId == cartHeader.CartHeaderId);

                    if(cartDetail is null)
                    {
                        // create cart details
                        model.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                        await _context.CartDetails.AddAsync(_mapper.Map<CartDetail>(model.CartDetails.First()));
                        await _context.SaveChangesAsync();  
                    }
                    else
                    {
                        // update count in cart details
                        model.CartDetails.First().Count += cartDetail.Count;
                        model.CartDetails.First().CartDetailsId = cartDetail.CartDetailsId;
                        model.CartDetails.First().CartHeaderId= cartDetail.CartHeaderId;
                        _context.CartDetails.Update(_mapper.Map<CartDetail>(model.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                }

                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                throw;
            }

            return _response;
        }
    }
}
