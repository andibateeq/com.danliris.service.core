﻿using Com.Danliris.Service.Core.Data.Migration.MigrationServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.WebApi.Controllers.v1.DataMigrations
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/migration/unit-of-measurements")]
    public class UnitOfMeasurementMigrationController : Controller
    {
        private readonly IUnitOfMeasurementMigrationService _service;

        public UnitOfMeasurementMigrationController(IUnitOfMeasurementMigrationService service)
        {
            _service = service;
        }

        [HttpGet("{startingNumber}/{numberOfBatch}")]
        public async Task<IActionResult> Get([FromRoute] int startingNumber, [FromRoute] int numberOfBatch)
        {
            try
            {
                var result = await _service.RunAsync(startingNumber, numberOfBatch);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
