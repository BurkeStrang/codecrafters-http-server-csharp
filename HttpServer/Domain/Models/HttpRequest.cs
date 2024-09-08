namespace codecrafters_http_server.HttpServer.Domain.Models;

public class HttpRequest
{
    public string Path { get; private set; } = "";
    public Dictionary<string, string> Headers { get; private set; } = [];
    private static readonly string[] Separator = ["\r\n", "\n"];
    public string Method { get; set; } = "";
    public string Body { get; set; } = "";

    public static HttpResponse CreateResponse(byte[] bufferRequest, string[] args)
    {
        HttpRequest request = ParseRequest(bufferRequest);

        bool isGzip =
            request.Headers.TryGetValue("Accept-Encoding", out string? contentEncoding)
            && contentEncoding.Contains("gzip");

        if (request.Path == "/")
        {
            return HttpResponse.Ok(isGzip: isGzip);
        }
        else if (request.Path.StartsWith("/echo"))
        {
            string echoValue = request.Path.Split('/').Last();
            return HttpResponse.Ok(isGzip: isGzip, body: echoValue);
        }
        else if (request.Path == "/user-agent")
        {
            return request.Headers.TryGetValue("User-Agent", out string? userAgent)
                ? HttpResponse.Ok(isGzip: isGzip, body: userAgent)
                : HttpResponse.Ok(isGzip: isGzip, body: "Unknown");
        }
        else
        {
            return request.Path.StartsWith("/files")
                ? HandleFileRequest(request.Path, args[1], request.Method, request.Body)
                : HttpResponse.NotFound();
        }
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
                Method = lines.FirstOrDefault()?.Split(' ')?.ElementAtOrDefault(0) ?? "",
                Body = lines.LastOrDefault() ?? "",
            };

        return request;
    }

    private static HttpResponse HandleFileRequest(
        string path,
        string fileDirectory,
        string method,
        string body
    )
    {
        string fileName = path.Split('/').Last();
        string filePath = System.IO.Path.Combine(fileDirectory, fileName);

        if (method == "POST")
        {
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            if (string.IsNullOrEmpty(body))
            {
                return HttpResponse.BadRequest();
            }

            body = body.TrimEnd('\0');
            File.WriteAllText(filePath, body);
            return HttpResponse.Created();
        }

        // Handle GET request (retrieving the file)
        return File.Exists(filePath) ? HttpResponse.File(filePath) : HttpResponse.NotFound();
    }
}
