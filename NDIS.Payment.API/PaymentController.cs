using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Services;

namespace NDIS.Payment.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
      _paymentService = paymentService;
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetByOrderId(string orderId)
    {
      var payment = await _paymentService.GetByOrderIdAsync(orderId);

      if (payment == null)
      {
        return NotFound();
      }

      return Ok(payment);
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PayOrderRequestDto request)
    {
      var result = await _paymentService.PayAsync(request);

      if (!result)
      {
        return BadRequest("Payment failed");
      }

      return Ok("Payment successful");
    }
  }
}
