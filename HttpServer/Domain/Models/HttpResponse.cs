namespace codecrafters_http_server.HttpServer.Domain.Models;

public class HttpResponse
{
    public int StatusCode { get; set; }
    public string? ReasonPhrase { get; set; }

    public override string ToString()
    {
        return $"HTTP/1.1 {StatusCode} {ReasonPhrase}\r\n\r\n";
    }

    public static HttpResponse CreateResponse(HttpRequest request)
    {
        return request.Path!.Equals("/")
            ? new HttpResponse { StatusCode = 200, ReasonPhrase = "OK" }
            : new HttpResponse { StatusCode = 404, ReasonPhrase = "Not Found" };
    }
}
