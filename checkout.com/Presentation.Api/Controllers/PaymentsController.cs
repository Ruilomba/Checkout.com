using System;
using System.Threading.Tasks;
using Checkout.com.PaymentGateway.Business.Services;
using Checkout.com.PaymentGateway.DTO.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string cardNumber, string merchantId, string customerId)
        {
            try
            {
                return this.Ok(await this.paymentService.SearchPayments(cardNumber, merchantId, customerId));
            }
            catch (Exception e)
            {
                //TODO LogException(e);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return this.Ok(await this.paymentService.GetPaymentById(id));
            }
            catch (Exception e)
            {
                //TODO LogException(e);
                return StatusCode(500);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> Create(PaymentRequest paymentRequest)
        {
            try
            {
                var result = await this.paymentService.ProcessPayment(paymentRequest);
                return this.Created($"api/payments/{result.PaymentId}", result);
            }
            catch (Exception e)
            {
                //TODO LogException(e);
                return StatusCode(500);
            }
        }
    }
}