using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Repositories;
using NDIS.Payment.API.Services;

namespace NDIS.Payment.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentController : ControllerBase
  {
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IWebHostEnvironment _env;
    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger, IPaymentRepository paymentRepository,IWebHostEnvironment webHostEnvironment)
    {
      _paymentService = paymentService;
      _logger = logger;
      _paymentRepository = paymentRepository;
      _env = webHostEnvironment;
    }

    [HttpGet("by-order/{orderId}")]
    public async Task<IActionResult> GetPaymentByOrderId(string orderId)
    {
      var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);

      if (payment == null)
      {
        return NotFound(new
        {
          message = "Payment is still being prepared.",
          orderId
        });
      }

      return Ok(payment);
    }

    [HttpPost("dev/mark-succeeded/{orderId}")]
    public async Task<IActionResult> DevMarkSucceeded(string orderId)
    {
      if (!_env.IsDevelopment())
      {
        return NotFound();
      }

      var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

      if (payment == null)
      {
        return NotFound(new
        {
          message = "Payment not found.",
          orderId
        });
      }

      if (string.IsNullOrWhiteSpace(payment.StripePaymentIntentId))
      {
        return BadRequest(new
        {
          message = "StripePaymentIntentId is missing.",
          orderId
        });
      }

      await _paymentService.HandlePaymentSucceededAsync(
          stripeEventId: $"evt_dev_payment_succeeded_{payment.PaymentId}",
          stripePaymentIntentId: payment.StripePaymentIntentId);

      return Ok(new
      {
        message = "Payment marked as succeeded for development testing.",
        payment.PaymentId,
        payment.OrderId,
        payment.StripePaymentIntentId
      });
    }
  }
  
}
