﻿namespace EFCORE.Contract.Shared;

public class Result
{

    public Result(int statusCode, bool success, params Error[] details)
    {
        StatusCode = statusCode;
        Errors = details.ToList();
        IsSuccess = success;
    }

    public Result(int statusCode, bool success)
    {
        StatusCode = statusCode;
        IsSuccess = success;
        Errors = new();
    }

    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public List<Error> Errors { get; set; }

    public static Result Failure(int statusCode = 400, params Error[] errors)
    {
        return new(statusCode, false, errors);
    }

    public static Result Success(int statusCode = 200)
    {
        return new(statusCode, true);
    }
}

public class Result<T> : Result
{
    public Result(T data, int statusCode, bool isSuccess) : base(statusCode, isSuccess)
    {
        Data = data;
    }

    public Result(T data, int statusCode, bool isSuccess, params Error[] errors) : base(statusCode, isSuccess, errors)
    {
        Data = data;
    }

    public Result(int statusCode, params Error[] errors) : base(statusCode, false, errors)
    { }

    public T? Data { get; set; }

    public static Result<T> Success(T data, int statusCode = 200)
    {
        return new(data, statusCode, true);
    }

    public static Result<T> Failure(T data, int statusCode = 400, params Error[] errors)
    {
        return new(data, statusCode, false, errors);
    }
    public new static Result<T> Failure(int statusCode = 400, params Error[] errors)
    {
        return new(statusCode, errors);
    }
}

public record Error(string Code, string? Message = null);
