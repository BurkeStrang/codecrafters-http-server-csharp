using System.IO.Compression;

namespace codecrafters_http_server.HttpServer.Domain.Models;

public sealed class HttpResponse(string responseString)
{
    private readonly List<byte> _responseBytes = new(Encoding.UTF8.GetBytes(responseString));

    public HttpResponse AddHeader(string httpHeader, string value)
    {
        string header = $"{httpHeader}: {value}\r\n";
        _responseBytes.AddRange(Encoding.UTF8.GetBytes(header));
        return this;
    }

    public HttpResponse AddBody(string body)
    {
        _responseBytes.AddRange(Encoding.UTF8.GetBytes(body));
        return this;
    }

    public HttpResponse AddBody(byte[] body)
    {
        _responseBytes.AddRange(body);
        return this;
    }

    public HttpResponse AddCrlf()
    {
        _responseBytes.AddRange(Encoding.UTF8.GetBytes("\r\n"));
        return this;
    }

    public byte[] ToBytes()
    {
        return [.. _responseBytes];
    }

    public static byte[] GZip(string? body)
    {
        byte[] input = Encoding.UTF8.GetBytes(body!);
        using MemoryStream output = new();
        using (GZipStream gZipStream = new(output, CompressionMode.Compress))
        {
            gZipStream.Write(input, 0, input.Length);
        }
        return output.ToArray();
    }

    public static HttpResponse Ok(bool? isGzip = false, string? body = null)
    {
        if (isGzip == true && body != null)
        {
            byte[] compressedBody = GZip(body);

            return new HttpResponse(HttpStatusLine.Ok)
                .AddHeader(HttpHeader.ContentType, "text/plain")
                .AddHeader(HttpHeader.ContentEncoding, "gzip")
                .AddHeader(HttpHeader.ContentLength, compressedBody.Length.ToString())
                .AddCrlf()
                .AddBody(compressedBody);
        }
        else if (body != null)
        {
            return new HttpResponse(HttpStatusLine.Ok)
                .AddHeader(HttpHeader.ContentType, "text/plain")
                .AddHeader(HttpHeader.ContentLength, body.Length.ToString())
                .AddCrlf()
                .AddBody(body);
        }
        return new HttpResponse(HttpStatusLine.Ok).AddCrlf();
    }

    public static HttpResponse File(string filePath)
    {
        string fileContent = System.IO.File.ReadAllText(filePath);
        return new HttpResponse(HttpStatusLine.Ok)
            .AddHeader(HttpHeader.ContentType, "application/octet-stream")
            .AddHeader(HttpHeader.ContentLength, fileContent.Length.ToString())
            .AddCrlf()
            .AddBody(fileContent);
    }

    public static HttpResponse NotFound()
    {
        return new HttpResponse(HttpStatusLine.NotFound).AddCrlf();
    }

    public static HttpResponse Created()
    {
        return new HttpResponse(HttpStatusLine.Created).AddCrlf();
    }

    public static HttpResponse BadRequest()
    {
        return new HttpResponse(HttpStatusLine.BadRequest).AddCrlf();
    }
}
