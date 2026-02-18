namespace NDIS.Shared.Common.Extensions
{
    public class BusinessException : Exception
    {
        public string? ErrorCode { get; }

        public BusinessException(string message, string? errorCode = "BusinessError") : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
