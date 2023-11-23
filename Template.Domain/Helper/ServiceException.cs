using System.Net;

public class ServiceException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }
    public bool LogError { get; private set; }
    public ServiceException(string message, Exception? innerException = null) : this(message, HttpStatusCode.BadRequest, false, innerException) { }
    public ServiceException(string message, HttpStatusCode statusCode, Exception? innerException = null) : this(message, statusCode, false, innerException) { }
    public ServiceException(string message, bool log, Exception? innerException = null) : this(message, HttpStatusCode.BadRequest, log, innerException) { }
    public ServiceException(string message, HttpStatusCode statusCode, bool log, Exception? innerException = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        LogError = log;
    }
}