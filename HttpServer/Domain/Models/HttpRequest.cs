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
        HttpResponse response;

        if (request.Path == "/")
        {
            response = HttpResponse.Ok();
        }
        else if (request.Path.StartsWith("/echo"))
        {
            var echoValue = request.Path.Split('/').Last();
            response = HttpResponse.Ok(echoValue);
        }
        else if (request.Path == "/user-agent")
        {
            if (request.Headers.TryGetValue("User-Agent", out string? userAgent))
            {
                response = HttpResponse.Ok(userAgent);
            }
            else
            {
                response = HttpResponse.Ok("Unknown");
            }
        }
        else if (request.Path.StartsWith("/files"))
        {
            response = HandleFileRequest(request.Path, args[1], request.Method, request.Body);
        }
        else
        {
            response = HttpResponse.NotFound();
        }
        return response;
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
                Body = lines[^1].Trim()
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
            // Ensure the directory exists
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            // Validate the body is not null or empty
            if (string.IsNullOrEmpty(body))
            {
                return HttpResponse.BadRequest();
            }

            // Write the file
            File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(body));
            return HttpResponse.Created();
        }

        // Handle GET request (retrieving the file)
        if (File.Exists(filePath))
        {
            return HttpResponse.File(filePath);
        }
        else
        {
            return HttpResponse.NotFound();
        }
    }
}
