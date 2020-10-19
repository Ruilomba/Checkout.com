using System;
using System.Threading.Tasks;
using Checkout.com.Common.Logging;
using Checkout.com.Common.Logging.Implementations;
using Checkout.com.PaymentGateway.Business.Services;
using Checkout.com.PaymentGateway.DTO.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        private readonly ILog logger;

        public PaymentsController(IPaymentService paymentService, ILog logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string cardNumber, string merchantId, string customerId)
        {
            try
            {
                return this.Ok(await this.paymentService.SearchPayments(cardNumber, merchantId, customerId));
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error searching for payments", () => new 
                {
                    CardNumber = cardNumber,
                    MerchantId = merchantId,
                    CustomerId = customerId
                },
                exception);
                return StatusCode(500, exception);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return this.Ok(await this.paymentService.GetPaymentById(id));
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error geting payment", () => new
                {
                    Id = id,
                },
                exception);
                return StatusCode(500, exception);
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
            catch (Exception exception)
            {
                this.logger.LogError("Error processing payment", () => new
                {
                    PaymentRequest = paymentRequest,
                },
                exception);
                return StatusCode(500, exception);
            }
        }
    }
}