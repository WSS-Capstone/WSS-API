namespace L.Core.Model;

public class ApiResponse<T>
{
    public ApiResponse()
    {
        Succeeded = true;
    }

    public ApiResponse(T data)
    {
        Succeeded = true;
        Data = data;
    }

    public ApiResponse(string message)
    {
        Succeeded = false;
        Message = message;
    }

    public ApiResponse(Exception exception)
    {
        Succeeded = false;
        Exception = exception;
    }

    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public Exception? Exception { get; set; }
}