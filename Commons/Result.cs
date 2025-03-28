using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aspnetcore.Commons
{
    public class Result
    {
        public string Message { get; set; } = default!;
        public string Details { get; set; } = default!;
        public int StatusCode { get; set; } = default!;
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; } = default;
        public static Result<T> Create(T data, string details, string message = "Ok", int statusCode = 200)
        {
            var result = new Result<T>();
            result.Data = data;
            result.Message = message;
            result.StatusCode = statusCode;
            result.Details = details;
            return result;
        }
    }
}
