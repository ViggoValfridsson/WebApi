using System.Net;

namespace WebApi.Models.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorMessage { get; set; } = null!;

    public ApiException(HttpStatusCode statusCode, string errorMessage) 
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}
