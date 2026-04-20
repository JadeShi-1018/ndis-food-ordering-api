using Microsoft.AspNetCore.Mvc;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Services;

namespace NDIS.Payment.API.Controllers
{
  [ApiController]
  [Route("api/payment")]
  public class PaymentController : ControllerBase
  {
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
      _paymentService = paymentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment(CreatePaymentRequestDto request)
    {
      var result = await _paymentService.CreatePaymentAsync(request);
      return Ok(result);
    }


    [HttpPost("pay/{paymentId}")]
    public async Task<IActionResult> Pay(string paymentId, PayPaymentRequestDto request)
    {
      var result = await _paymentService.PayAsync(paymentId, request);
      return Ok(result);
    }
  }
}