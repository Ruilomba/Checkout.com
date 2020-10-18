using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {


        [HttpGet("{paymentId}")]
        public async Task<IActionResult> Get(string paymentId)
        {
            return this.Ok("success");
        }

        [HttpPost()]
        public async Task<IActionResult> Create(string paymentId)
        {
            return this.Ok("success");
        }
    }
}
