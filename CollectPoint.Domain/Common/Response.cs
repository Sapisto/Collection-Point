namespace CollectPoint.Domain.Common;

public class Response<T>
{
    public Response() { }

    // Success
    public Response(T data, string message = "Success")
    {
        Succeeded = true;
        Code = 200;
        Message = message;
        Data = data;
    }

    // Failure
    public Response(string message, int code, IEnumerable<string>? errors = null)
    {
        Succeeded = false;
        Code = code;
        Message = message;
        Errors = errors;
    }

    public bool Succeeded { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public PageMeta? PageMeta { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public class PageMeta
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}
