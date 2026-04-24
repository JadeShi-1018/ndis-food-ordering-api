using NDIS.Payment.API.Domain;
using NDIS.Payment.API.Domain.StateMachines;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Enums;
using NDIS.Payment.API.Repositories;
using NDIS.Payment.API.Repository;
using NDIS.Payment.API.ServiceClient;
using Stripe;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Services
{
  public class PaymentService : IPaymentService
  {
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    private readonly IOrderServiceClient _orderServiceClient;

    public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger, IOrderServiceClient orderServiceClient)
    {
      _paymentRepository = paymentRepository;
      _logger = logger;
      _orderServiceClient = orderServiceClient;
    }

    public async Task<CreatePaymentResponseDto> CreatePaymentAsync(CreatePaymentRequestDto request)
    {
      // incase pay several times
      var existing = await _paymentRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey);

      if (existing != null)
      {
        return new CreatePaymentResponseDto
        {
          PaymentId = existing.PaymentId,
          PaymentStatus = existing.PaymentStatus.ToString()
        };
      }

      // create payment
      var payment = new PaymentEntity
      {
        PaymentId = Guid.NewGuid().ToString(),
        OrderId = request.OrderId,
        UserId = request.UserId,
        Amount = request.Amount,
        PaymentStatus = PaymentStatus.Pending,
        IdempotencyKey = request.IdempotencyKey,
        CreatedAt = DateTime.UtcNow
      };

      await _paymentRepository.AddAsync(payment);
      await _paymentRepository.SaveChangesAsync();

      return new CreatePaymentResponseDto
      {
        PaymentId = payment.PaymentId,
        PaymentStatus = payment.PaymentStatus.ToString()
      };
    }

    public async Task<PayPaymentResponseDto> PayAsync(string paymentId, PayPaymentRequestDto request)
    {
      var payment = await _paymentRepository.GetByIdAsync(paymentId);
      


      if (payment == null)
      {
        throw new Exception("Payment not found.");
      }

      Console.WriteLine($"Current Payment is {payment.PaymentId}");
      Console.WriteLine($"Current Payment status is {payment.PaymentStatus}");

      if (payment.PaymentStatus != PaymentStatus.Pending)
      {
        throw new Exception("This payment is not in pending status.");
      }
      try { 

      var options = new PaymentIntentCreateOptions
      {
        Amount = (long)(payment.Amount * 100),
        Currency = "aud",
        PaymentMethod = "pm_card_visa",
        Confirm = true,
        AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
        {
          Enabled = true,
          AllowRedirects = "never"
        }
      };

      var service = new PaymentIntentService();
      var intent = await service.CreateAsync(options);

      //save Stripe Intent id for debug
         payment.StripePaymentIntentId = intent.Id;

      if (intent.Status == "succeeded")
      {
          payment.ChangeStatus( PaymentStatus.Succeeded);
        payment.PaymentMethod = "Stripe";
        payment.UpdatedAt = DateTime.UtcNow;

        await _paymentRepository.SaveChangesAsync();

        await _orderServiceClient.UpdateOrderStatusAsync(
            payment.OrderId,
            new UpdateOrderStatusRequestDto
            {
              OrderStatus = "Paid"
            });

        return new PayPaymentResponseDto
        {
          PaymentId = payment.PaymentId,
          PaymentStatus = payment.PaymentStatus.ToString(),
          Message = "Payment successful."
        };
      }

        payment.ChangeStatus(PaymentStatus.Failed);
        payment.PaymentMethod = "Stripe";
      payment.UpdatedAt = DateTime.UtcNow;

      await _paymentRepository.SaveChangesAsync();

      await _orderServiceClient.UpdateOrderStatusAsync(
          payment.OrderId,
          new UpdateOrderStatusRequestDto
          {
            OrderStatus = "Failed"
          });

      throw new Exception($"Stripe payment did not succeed. Status: {intent.Status}");
    }
    catch (StripeException ex)
    {
        _logger.LogError(ex, "Stripe payment failed for PaymentId: {PaymentId}", paymentId);

        payment.PaymentStatus = PaymentStatus.Failed;
        payment.PaymentMethod = "Stripe";
        payment.UpdatedAt = DateTime.UtcNow;

        await _paymentRepository.SaveChangesAsync();

    await _orderServiceClient.UpdateOrderStatusAsync(
        payment.OrderId,
            new UpdateOrderStatusRequestDto
            {
                OrderStatus = "Failed"
            });

        throw new Exception($"Stripe payment failed: {ex.StripeError?.Message ?? ex.Message}");
}
}

  }
}