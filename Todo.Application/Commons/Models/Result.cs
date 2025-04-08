namespace Todo.Application.Commons.Models;
public class Result
{
    public Result(string message, string details, int statusCode, bool isSuccess)
    {
        Message = message;
        Details = details;
        StatusCode = statusCode;
        IsSuccess = isSuccess;
    }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }
    public int StatusCode { get; set; }

}

public class Result<T> : Result
{
    public Result(T data, string message, string details) : base(message, details, 200, true)
    {
        Data = data;
    }
    public T Data { get; set; }
}
