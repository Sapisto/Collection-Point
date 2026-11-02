using System.Net;

namespace CollectPoint.Core.Exceptions;

public class ApiException : Exception
{
    public ApiException(string message, int errorCode = 99,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
        Exception? innerException = null) : base(message, innerException)
    {
        ErrorCode = errorCode;
        HttpStatusCode = httpStatusCode;
    }

    public int ErrorCode { get; }
    public HttpStatusCode HttpStatusCode { get; }
}
