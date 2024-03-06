using Infrastructure.Models;

public static class ResponseFactory
{
    public static ResponseResult Ok(string message = null!)
    {
        return new ResponseResult
        {
            Message = message ?? "Succeeded.",
            StatusCode = StatusCode.OK,
        };
    }

    public static ResponseResult Ok(object obj, string message = null!)
    {
        return new ResponseResult
        {
            ContentResult = obj,
            Message = message ?? "Succeeded.",
            StatusCode = StatusCode.OK,
        };
    }

    public static ResponseResult Error(string message = null!)
    {
        return new ResponseResult
        {
            Message = message ?? "Failed.",
            StatusCode = StatusCode.ERROR,
        };
    }

    public static ResponseResult NotFound(string message = null!)
    {
        return new ResponseResult
        {
            Message = message ?? "Not found.",
            StatusCode = StatusCode.NOT_FOUND,
        };
    }

    public static ResponseResult Exists(string message = null!)
    {
        return new ResponseResult
        {
            Message = message ?? "Exists.",
            StatusCode = StatusCode.EXISTS,
        };
    }
}
