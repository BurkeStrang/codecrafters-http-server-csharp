namespace codecrafters_http_server.HttpServer.Domain.Models;

public class HttpResponse
{
    public int StatusCode { get; set; }
    public string? ReasonPhrase { get; set; }
    public string? Body { get; set; }

    public override string ToString()
    {
        if (Body is not null)
        {
            string headers = $"Content-Type: text/plain\r\nContent-Length: {Body.Length}";
            return $"HTTP/1.1 {StatusCode} {ReasonPhrase}\r\n{headers}\r\n\r\n{Body}";
        }
        else
        {
            return $"HTTP/1.1 {StatusCode} {ReasonPhrase}\r\n\r\n";
        }
    }

    public static HttpResponse CreateResponse(HttpRequest request)
    {
        if (request.Path!.Equals("/"))
        {
            return new HttpResponse { StatusCode = 200, ReasonPhrase = "OK" };
        }
        else if (request.Path.StartsWith("/echo/"))
        {
            return new HttpResponse
            {
                StatusCode = 200,
                ReasonPhrase = "OK",
                Body = request.Body,
            };
        }
        else
        {
            return new HttpResponse { StatusCode = 404, ReasonPhrase = "Not Found" };
        }
    }
}
