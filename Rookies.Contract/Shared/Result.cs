using System.Net;

namespace Rookies.Contract.Shared;
public class Result
{
    public Result() { }
    public Result(string message, int statusCode, string detail, bool success)
    {
        Message = message;
        StatusCode = statusCode;
        Detail = detail;
        Success = success;
    }
    public Result(string message, int statusCode, bool success)
    {
        Message = message;
        StatusCode = statusCode;
        Success = success; 
    }
    public bool Success { get; set; }
    public required string Message { get; set; }
    public required int StatusCode { get; set; }
    public string? Detail { get; set; }

}

public class Result<T> : Result
{
    public Result(T data, string message, int statusCode, string detail) : base(message, statusCode, detail, true)
    {
        Data = data;
    }
    public Result(T data, string message, int statusCode) : base(message, statusCode, true)
    {
        Data = data;
    }
    public Result(T data) : base(HttpStatusCode.OK.ToString(), 200, true)
    {
        Data = data;
    }
    public T Data { get; set; }
}
