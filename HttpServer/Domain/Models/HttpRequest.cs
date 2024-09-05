namespace codecrafters_http_server.HttpServer.Domain.Models;

public class HttpRequest
{
    public string? Method { get; set; }
    public string? Path { get; set; }
    public string? Body { get; set; }
    public string? Header { get; set; }

    public static HttpRequest Parse(byte[] bufferRequest)
    {

        string requestString = Encoding.ASCII.GetString(bufferRequest);
        string startLine = requestString.Split("\r\n")[0];
        string method = startLine.Split(" ")[0];
        string path = startLine.Split(" ")[1];
        string? body = path.Split("/")[^1] ?? null;
        string? header = requestString.Split("User-Agent: ")[1].Split("\r\n")[0] ?? null;

        return new HttpRequest
        {
            Method = method,
            Path = path,
            Body = body,
            Header = header
        };
    }
}
