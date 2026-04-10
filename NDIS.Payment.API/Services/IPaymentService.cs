using NDIS.Contracts.Events;
using NDIS.Payment.API.Dtos;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Services
{
  public interface IPaymentService
  {
    Task CreatePaymentFromOrderAsync(OrderCreatedEvent message);
    Task<PaymentResponseDto?> GetByOrderIdAsync(string orderId);
    Task<bool> PayAsync(PayOrderRequestDto payOrderRequestDto);
  }
}
