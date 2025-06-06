using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Services.DTO.DealModel;
using MomAndBaby.Services.DTO.ServicePackageModel;
using MomAndBaby.Services.DTO.UserModel;
using MomAndBaby.Services.Interface;
using MomAndBaby.Services.Services;

namespace MomAndBaby.API.Controllers
{
    [Route("api/service-package")]
    [ApiController]
    public class ServicePackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IDealService _dealService;
        public ServicePackageController(IPackageService packageService, IDealService dealService)
        {
            _packageService = packageService;
            _dealService = dealService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicePackageById(string id)
        {
            try
            {
                var result = await _packageService.GetPackageById(id);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetServicePackage(string? searchString, int pageIndex, int pageSize, bool isDescending, PagingPackageEnum pagingPackageEnum = PagingPackageEnum.CreateTime)
        {
            try
            {
                var result = await _packageService.GetPackageByPagination(pageIndex, pageSize, searchString, pagingPackageEnum, isDescending);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServicePackage([FromBody] PackageModel packageCreateDTO)
        {
            try
            {
                var result = await _packageService.CreateServicePackage(packageCreateDTO);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicePackage(string id, [FromBody] UpdatePackageModel packageUpdateDTO)
        {
            try
            {
                var result = await _packageService.UpdatePackage(id, packageUpdateDTO);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("deal")]
        public async Task<IActionResult> GetDeals()
        {
            try
            {
                var result = await _dealService.GetDealAll();
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("deal")]
        public async Task<IActionResult> CreateDeal([FromBody] CreateDealModel dealModel)
        {
            try
            {
                var result = await _dealService.CreateDeal(dealModel);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("deal/{id}")]
        public async Task<IActionResult> UpdateDeal(string id, UpdateDealModel dealModel)
        {
            try
            {
                var result = await _dealService.UpdateDeal(id, dealModel);
                return Ok(result);
            }
            catch (BaseException ex)
            {
                return StatusCode(ex.ErrorCode, ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
