namespace mvc_todolist.Commons
{
    public class Result
    {
        public Result(string message, string detail, int statusCode)
        {
            this.Message = message;
            this.Detail = detail;
            this.StatusCode = statusCode;
        }
        public string Message { get; set; } = default!;
        public string Detail { get; set; } = default!;
        public int StatusCode { get; set; } = default!;
    }

    public class Result<T> : Result
    {
        public Result(string Message, string Detail, int StatusCode) : base(Message, Detail, StatusCode)
        {
        }

        public T? Data { get; set; } = default;
        public static Result<T> Create(T data, string details, string message = "Ok", int statusCode = 200)
        {
            
            var result = new Result<T>(message, details, statusCode);
            result.Data = data;
            return result;
        }
    }
}
