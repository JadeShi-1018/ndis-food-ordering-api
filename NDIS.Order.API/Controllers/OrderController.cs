using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDIS.Order.API.Common;
using NDIS.Order.API.Dtos;
using NDIS.Order.API.Services;

namespace NDIS.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrderController : ControllerBase
    {

      private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

      public OrderController(IOrderService orderService, ILogger<OrderController>logger)
      {
        _orderService = orderService;
      _logger = logger;
      }

      [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderRequestDto request)
      {
      try
      {
        var createdOrder = await _orderService.CreateOrderAsync(request, User);
        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
      }
      catch (UnauthorizedAccessException ex)
      {
        _logger.LogWarning(ex, "Unauthorized when creating order.");
        return Unauthorized(ApiResponse<string>.Fail("UNAUTHORIZED", ex.Message));
      }
      catch (ArgumentException ex)
      {
        _logger.LogWarning(ex, "Invalid request when creating order.");
        return BadRequest(ApiResponse<string>.Fail("INVALID_REQUEST", ex.Message));
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogWarning(ex, "Business conflict when creating order.");
        return Conflict(ApiResponse<string>.Fail("CONFLICT", ex.Message));
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unexpected error when creating order.");
        return StatusCode(500, ApiResponse<string>.Fail("INTERNAL_SERVER_ERROR", "An unexpected error occurred."));
      }
    }




      [HttpGet("{id}")]
      public async Task<ActionResult<OrderResponseDto>> GetOrderById(string id)
      {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
        {
          return NotFound($"Order with id {id} not found.");
        }

        return Ok(order);
      }

      [HttpGet("user/{userId}")]
      public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersByUserId(string userId)
      {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
      }
    }
  }
