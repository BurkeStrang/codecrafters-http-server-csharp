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

        // Split by line, ensuring at least one line exists
        string[] requestLines = requestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        if (requestLines.Length == 0)
        {
            throw new ArgumentException("Invalid HTTP request: missing start line.");
        }

        // Split the start line and check for at least two parts (method and path)
        string[] startLineParts = requestLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (startLineParts.Length < 2)
        {
            throw new ArgumentException("Invalid HTTP request: missing method or path.");
        }

        string method = startLineParts[0];
        string path = startLineParts[1];

        // Parse body safely, checking if the path contains enough parts
        string? body = path.Contains("/") ? path.Split("/").Last() : null;

        // Safely extract header, checking if "User-Agent" exists
        string? header = null;
        if (requestString.Contains("User-Agent: "))
        {
            var headerParts = requestString.Split("User-Agent: ", StringSplitOptions.None);
            if (headerParts.Length > 1)
            {
                var headerLines = headerParts[1]
                    .Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                header = headerLines.Length > 0 ? headerLines[0] : null;
            }
        }

        return new HttpRequest
        {
            Method = method,
            Path = path,
            Body = body,
            Header = header,
        };
    }
}
