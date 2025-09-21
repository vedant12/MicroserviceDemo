using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

            var dbCoupon = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.CouponCode.ToLower() ==  code.ToLower());

            if (dbCoupon == null) return NotFound();

            response.Result = mapper.Map<CouponDto>(dbCoupon);

            return Ok(response);
        }
    }
}
