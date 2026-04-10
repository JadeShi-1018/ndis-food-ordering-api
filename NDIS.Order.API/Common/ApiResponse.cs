namespace NDIS.Order.API.Common
{
  public class ApiResponse<T>
  {
    public bool Succeed { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
  

 public static ApiResponse<T> Success(T data, string message = "Success")
    {
      return new ApiResponse<T>
      {
        Succeed = true,
        Message = message,
        Data = data
      };
    }

    public static ApiResponse<T> Fail(string errorCode, string errorMessage)
    {
      return new ApiResponse<T>
      {
        Succeed = false,
        ErrorCode = errorCode,
        ErrorMessage = errorMessage
      };
    }
  }
}
