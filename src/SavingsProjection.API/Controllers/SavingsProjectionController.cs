﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SavingsProjection.API.Services.Abstract;
using SavingsProjection.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SavingsProjection.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsProjectionController : ControllerBase
    {
        private readonly IProjectionCalculator calculator;
        public readonly IConfiguration configuration;


        public SavingsProjectionController(IProjectionCalculator calculator, IConfiguration configuration)
        {
            this.calculator = calculator;
            this.configuration = configuration;
        }

        // GET: api/SavingsProjection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterializedMoneyItem>>> GetSavingsProjection(DateTime? from, DateTime? to, bool onlyInstallment = false)
        {
            return (List<MaterializedMoneyItem>)await calculator.CalculateAsync(from, to, false, onlyInstallment);
        }


        [HttpPost("ToHistory")]
        public async Task<ActionResult> PostSavingsProjectionToHistory()
        {
            await calculator.SaveProjectionToHistory();
            return Ok();
        }

        [HttpGet("Backup")]
        public async Task<ActionResult> GetBackup()
        {
            return File(await System.IO.File.ReadAllBytesAsync(configuration["DatabasePath"]), "application/octet-stream", "Database.db");
        }

    }
}
