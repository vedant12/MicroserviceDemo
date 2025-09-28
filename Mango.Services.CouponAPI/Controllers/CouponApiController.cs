using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponApiController(AppDbContext _dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseDto<CouponDto>>>> GetAllCoupons()
        {
            var response = new ResponseDto<List<CouponDto>>();

            var dbCoupons = await _dbContext.Coupons.ToListAsync();

            response.Result = mapper.Map<List<CouponDto>>(dbCoupons);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseDto<CouponDto>>> GetCouponById(int id)
        {
            var response = new ResponseDto<CouponDto>();

            var dbCoupon = await _dbContext.Coupons.FindAsync(id);

            if (dbCoupon == null) return NotFound();

            response.Result = mapper.Map<CouponDto>(dbCoupon);

            return Ok(response);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<ResponseDto<CouponDto>>> GetCouponByCode(string code)
        {
            var response = new ResponseDto<CouponDto>();

            var dbCoupon = await _dbContext.Coupons.FirstAsync(x => x.CouponCode.ToLower() == code.ToLower());

            if (dbCoupon == null) return NotFound();

            response.Result = mapper.Map<CouponDto>(dbCoupon);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto<Coupon>>> CreateCoupon(CouponDto model)
        {
            var response = new ResponseDto<Coupon>();

            var coupon = mapper.Map<Coupon>(model);

            await _dbContext.Coupons.AddAsync(coupon);

            await _dbContext.SaveChangesAsync();

            response.Result = coupon;

            return Created("Coupon added successfully", response);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto<Coupon>>> UpdateCoupon(CouponDto model)
        {
            var response = new ResponseDto<Coupon>();

            var coupon = mapper.Map<Coupon>(model);

            _dbContext.Update(coupon);

            await _dbContext.SaveChangesAsync();

            response.Result = coupon;

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto<Coupon>>> DeleteCoupon(int id)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.CouponId == id);

            if (coupon == null) return NotFound();

            var response = new ResponseDto<Coupon>();

            response.Result = null;

            _dbContext.Coupons.Remove(coupon);

            await _dbContext.SaveChangesAsync();

            return Ok(response);
        }
    }
}
