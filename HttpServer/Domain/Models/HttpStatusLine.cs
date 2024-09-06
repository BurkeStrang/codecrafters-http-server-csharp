namespace codecrafters_http_server.HttpServer.Domain.Models;

public static class HttpStatusLine
{
    public const string Ok = "HTTP/1.1 200 OK\r\n";
    public const string NotFound = "HTTP/1.1 404 Not Found\r\n";
    public const string Created = "HTTP/1.1 201 Created\r\n";
    public const string BadRequest = "HTTP/1.1 400 Bad Request\r\n";
}
