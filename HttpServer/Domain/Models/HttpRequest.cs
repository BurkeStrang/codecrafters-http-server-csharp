namespace codecrafters_http_server.HttpServer.Domain.Models;

public class HttpRequest
{
    public string Path { get; private set; } = "";
    public Dictionary<string, string> Headers { get; private set; } = [];
    private static readonly string[] Separator = ["\r\n", "\n"];

    public static HttpResponse CreateResponse(byte[] bufferRequest, string[] args)
    {
        HttpRequest request = ParseRequest(bufferRequest);

        return request.Path switch
        {
            "/" => HttpResponse.Ok(),
            string path when path.StartsWith("/echo") => HttpResponse.Ok(path.Split('/').Last()),
            "/user-agent" => HttpResponse.Ok(
                request.Headers.TryGetValue("User-Agent", out string? userAgent)
                    ? userAgent
                    : "Unknown"
            ),
            string path when path.StartsWith("/files") => HandleFileRequest(path, args[1]),
            _ => HttpResponse.NotFound(),
        };
    }

    private static HttpRequest ParseRequest(byte[] bufferRequest)
    {
        string requestString = Encoding.UTF8.GetString(bufferRequest);
        string[] lines = requestString.Split(Separator, StringSplitOptions.None);
        HttpRequest request =
            new()
            {
                Path = lines.FirstOrDefault()?.Split(' ')?.ElementAtOrDefault(1) ?? "/",
                Headers = lines
                    .Skip(1)
                    .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(':', 2))
                    .Where(parts => parts.Length == 2)
                    .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim()),
            };

        return request;
    }

    private static HttpResponse HandleFileRequest(string path, string fileDirectory)
    {
        string fileName = path.Split('/').Last();
        string filePath = System.IO.Path.Combine(fileDirectory, fileName);
        return File.Exists(filePath) ? HttpResponse.File(filePath) : HttpResponse.NotFound();
    }
}
