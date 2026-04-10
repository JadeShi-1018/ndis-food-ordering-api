using AutoMapper;
using NDIS.Order.API.Domain.Entities;
using NDIS.Order.API.Repositories;
using NDIS.Order.API.Dtos;
using OrderEntity = NDIS.Order.API.Domain.Entities.Order;
using System.Security.Claims;
using NDIS.Order.API.ServiceClient;
using NDIS.Order.API.Utilities;
using NDIS.Order.API.Domain.Enums;
using System.Text.Json;
using NDIS.Order.API.Service.Idempotency;
using NDIS.Contracts.Events;
using NDIS.Order.API.Repository;
using StackExchange.Redis;
using MassTransit.SagaStateMachine;
namespace NDIS.Order.API.Services
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IUserServiceClient _userServiceClient;
    private readonly IServiceServiceClient _serviceServiceClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<OrderService> _logger;
    private readonly IIdempotencyService _idempotencyService;
    private readonly IOrderEventRepository _orderEventRepository;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserServiceClient userServiceClient, IServiceServiceClient serviceServiceClient, IHttpContextAccessor httpContextAccessor, ILogger<OrderService> logger, IIdempotencyService idempotencyService, IOrderEventRepository orderEventRepository)
    {
      _orderRepository = orderRepository;
      _mapper = mapper;
      _userServiceClient = userServiceClient;
      _serviceServiceClient = serviceServiceClient;
      _httpContextAccessor = httpContextAccessor;
      _logger = logger;
      _idempotencyService = idempotencyService;
      _orderEventRepository = orderEventRepository;
   
    }

    private OrderEvent BuildOrderCreatedEvent(OrderEntity order)
    {
      var payload = new OrderCreatedEvent
      {
        OrderId = order.OrderId,
        UserId = order.UserId,
        CustomerName = order.CustomerName,
        ProviderId = order.ProviderId,
        ProviderServiceId = order.ProviderServiceId,
        ProviderServiceName = order.ProviderServiceName,
        CategoryId = order.CategoryId,
        CategoryName = order.CategoryName,
        MenuId = order.MenuId,
        MenuName = order.MenuName,
        PeriodName = order.PeriodName,
        Quantity = order.Quantity,
        UnitPrice = order.UnitPrice,
        OrderPrice = order.OrderPrice,
        StartDate = order.StartDate,
        EndDate = order.EndDate,
        IdempotencyKey = order.IdempotencyKey,
        CreatedAt = order.CreatedAt
       
      };

      return new OrderEvent
      {
        OrderEventId = Guid.NewGuid().ToString(),
        OrderId = order.OrderId,
        EventType = OrderEventType.OrderCreated,
        EventStatus = OrderEventStatus.Pending,
        Payload = JsonSerializer.Serialize(payload),
        RetryCount = 0,
        EventTimestamp = DateTime.UtcNow
      };
    }



    public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request, ClaimsPrincipal user)
    {
      // 1.  JWT --> userId
      var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      if (string.IsNullOrEmpty(userId))
      {
        throw new UnauthorizedAccessException("Invalid token: userId not found.");
      }

      var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        throw new UnauthorizedAccessException("Authorization header is missing.");
      }

      var token = authHeader.Substring("Bearer ".Length).Trim();

      //check redis to see if this order is processing or not --> idempotency

      var idempotencyKey = string.IsNullOrWhiteSpace(request.IdempotencyKey)
          ? Guid.NewGuid().ToString()
          : request.IdempotencyKey.Trim();

      var redisKey = $"order:idempotency:{userId}:{idempotencyKey}";

      var idemResult = await _idempotencyService.TryStartAsync(redisKey, TimeSpan.FromMinutes(10));

      if (!idemResult.Acquired)
      {
        if (idemResult.IsProcessing)
        {
          throw new InvalidOperationException("This request is already being processed.");
        }

        if (!string.IsNullOrWhiteSpace(idemResult.ExistingOrderId))
        {
          var existingOrder = await _orderRepository.GetOrderByIdAsync(idemResult.ExistingOrderId);

          if (existingOrder != null)
          {
            return _mapper.Map<OrderResponseDto>(existingOrder);
          }
        }
        throw new InvalidOperationException("Duplicate request detected.");
      }
      try
      {

        // 3. call User Service to get current user info
        var userInfo = await _userServiceClient.GetUserByIdAsync(token);
        _logger.LogInformation("userInfo is {@userInfo}", JsonSerializer.Serialize(userInfo));

        if (userInfo == null)
        {
          throw new Exception("Failed to retrieve user info.");
        }

        // 4. call Service Service to get menbu info
        var menuInfo = await _serviceServiceClient.GetMenuOrderInfoAsync(
            request.ProviderServiceId,
            request.CategoryId,
            request.MenuId);
        _logger.LogInformation("MenuInfo is {@menuInfo}", JsonSerializer.Serialize(menuInfo));

        if (menuInfo == null)
        {
          throw new Exception("Failed to retrieve menu info.");
        }

        // 5. calculate quantity & order total price
        var quantity = MealCalculator.CalculateMealCount(request.StartDate, request.EndDate, menuInfo.PeriodName);
        var totalPrice = quantity * menuInfo.UnitPrice;

        // 6. OrderEntity
        var order = new OrderEntity
        {
          OrderId = Guid.NewGuid().ToString(),
          UserId = userId,
          CustomerName = userInfo.Name,

          ProviderId = menuInfo.ProviderId,
          ProviderServiceName = menuInfo.ProviderServiceName,
          ProviderServiceId = menuInfo.ProviderServiceId,
          ProviderPhoneNumber = menuInfo.ProviderPhoneNumber ?? string.Empty,

          IdempotencyKey = idempotencyKey,

          CategoryId = menuInfo.CategoryId,
          CategoryName = menuInfo.CategoryName,

          MenuId = menuInfo.MenuId,
          MenuName = menuInfo.MenuName,
          MenuDescription = menuInfo.MenuDescription ?? string.Empty,

          PeriodName = menuInfo.PeriodName,
          Quantity = quantity,
          UnitPrice = menuInfo.UnitPrice,
          OrderPrice = totalPrice,

          StartDate = request.StartDate,
          EndDate = request.EndDate,

          PaymentId = null,

          DeliveryAddress = request.DeliveryAddress,
          CustomerContactNumber = userInfo.PhoneNumber,


          OrderStatus = OrderStatus.PendingPayment,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        var orderEvent = BuildOrderCreatedEvent(order);

        await _orderRepository.CreateOrderAsync(order);




        await _orderEventRepository.AddAsync(orderEvent);
        await _orderRepository.SaveChangesAsync();

        // 7.mark idempotency success
        await _idempotencyService.MarkSuccessAsync(redisKey, order.OrderId, TimeSpan.FromHours(24));
        return _mapper.Map<OrderResponseDto>(order);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "CreateOrder failed, releasing idempotency key");

        // when failling in all business service, don't need to wait for TTL ends
        await _idempotencyService.ReleaseAsync(redisKey);

        throw;
      }

        

      }
      




    public async Task<OrderResponseDto?> GetOrderByIdAsync(string orderId)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId);

      if (order == null)
      {
        return null;
      }

      return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<List<OrderResponseDto>> GetOrdersByUserIdAsync(string userId)
    {
      var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
      return _mapper.Map<List<OrderResponseDto>>(orders);
    }
  }
}