using System.Net;

namespace HiveFS.Shared;

public record ApiResult<T>
{
    public T Data { get; set; } 
    public bool Succeeded { get; set; }
    
    public string Message { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }

}