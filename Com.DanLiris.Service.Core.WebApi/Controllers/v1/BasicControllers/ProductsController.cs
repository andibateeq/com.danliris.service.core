﻿using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/products")]
    public class ProductsController : BasicController<ProductService, Product, ProductViewModel, CoreDbContext>
    {
        private static readonly string ApiVersion = "1.0";
        ProductService service;

        public ProductsController(ProductService service) : base(service, ApiVersion)
        {
            this.service = service;
        }

        [HttpGet("byId")]
        public IActionResult GetByIds([Bind(Prefix = "productList[]")]List<string> productList)
        {
            try
            {

                service.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                List<Product> Data = service.GetByIds(productList);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Data);

                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost("packing/create")]
        public async Task<IActionResult> PostPacking([FromBody] PackingModel packings)
        {
            try
            {
                service.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                bool success = await service.CreateProduct(packings);

                if (success)
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                    return NoContent();
                }
                else
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, "Failed to create product!")
                    .Fail();
                    return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("byProductionOrderNo")]
        public async Task<IActionResult> GetByProductionOrederNo([FromQuery] string productionOrderNo)
        {
            try
            {

                service.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                List<ProductViewModel> Data = await service.GetProductByProductionOrderNo(productionOrderNo);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Data);

                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
