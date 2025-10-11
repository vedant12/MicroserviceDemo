using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Dtos;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController(IMapper _mapper, AppDbContext _context, 
        IProductService _productService, ICouponService _couponService) : ControllerBase
    {
        private readonly ResponseDto _response = new();

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId))
                };

                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(await _context.CartDetails
                    .Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId).ToListAsync());

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon is not null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                throw;
            }

            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart(int cartDetailsId)
        {
            try
            {
                CartDetail cartDetails = await _context.CartDetails.FirstOrDefaultAsync(x => x.CartDetailsId == cartDetailsId);

                int totalCartItemCount = _context.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetails);

                if (totalCartItemCount == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);

                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                throw;
            }

            return _response;
        }

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

                    if (cartDetail is null)
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
                        model.CartDetails.First().CartHeaderId = cartDetail.CartHeaderId;
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

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon(CartDto model)
        {
            try
            {
                var cart = await _context.CartHeaders.FirstAsync(x => x.UserId == model.CartHeader.UserId);
                
                cart.CouponCode = model.CartHeader.CouponCode;
                
                _context.Update(cart);

                await _context.SaveChangesAsync();

                _response.Message = "Coupon Applied";
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon(CartDto model)
        {
            try
            {
                var cart = await _context.CartHeaders.FirstAsync(x => x.UserId == model.CartHeader.UserId);

                cart.CouponCode = "";

                _context.Update(cart);

                await _context.SaveChangesAsync();

                _response.Message = "Coupon Removed";
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
